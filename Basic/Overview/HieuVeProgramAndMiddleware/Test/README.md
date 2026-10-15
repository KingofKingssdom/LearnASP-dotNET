# Kiến thức về file Program.cs
- Trong ASP.NET Core, Program.cs đóng vai trò là điểm khởi chạy (Entry Point) của toàn bộ ứng dụng, và Request/Response Pipeline (Pipeline Middleware) chính là bộ máy xử lý mọi lượt truy cập gửi đến server.
## I. File Program.cs - trái tim của toàn bộ ứng dụng
- Kể từ .NET 6 trở đi, cấu hình dự án được tối giản trong duy nhất file Program.cs thông qua mô hình Minimal Hosting Model
. File này được chia làm 4 giai đoạn thực thi rõ rệt:
```C#
// -------------------------------------------------------------
// GIAI ĐOẠN 1: Tạo Builder & Đăng ký Dịch vụ (Service Container / DI)
// -------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// Đăng ký các Service vào hệ thống Dependency Injection (DI)
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(...);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------------------------------------------
// GIAI ĐOẠN 2: Build ứng dụng
// -------------------------------------------------------------
var app = builder.Build(); 

// -------------------------------------------------------------
// GIAI ĐOẠN 3: Cấu hình Request/Response Middleware (Pipeline)
// -------------------------------------------------------------
if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();      
app.UseRouting();          

app.UseAuthentication();
app.UseAuthorization();    

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

// -------------------------------------------------------------
// GIAI ĐOẠN 4: Chạy ứng dụng
// -------------------------------------------------------------
app.Run(); 
```
   + WebApplication.CreateBuilder(args): Khởi tạo builder chứa các cấu hình mặc định (Kestrel Server, Logging, appsettings.json, Environment Variables)
   + builder.Services: Nơi đăng ký các lớp phụ thuộc (DI) thông qua IServiceCollection.
   + builder.Build(): Đóng gói các cấu hình và tạo ra đối tượng app thuộc kiểu WebApplication
   + app.Use...() / app.Map...(): Thiết lập các đoạn mã xử lý request (Middleware).
   + app.Run(): Lắng nghe cổng HTTP/HTTPS và bắt đầu nhận Request
## II. Khái niệm về middlewade và pipeline
- Middleware: là nơi để xử lý request đi qua cần làm gì, ví dụ đăng nhập, phân quyền
- Pipiline: là một chuỗi các middleware nối với nhau tạo thành một pipeline
   + Khi client gửi một HTTP Request đến server, request sẽ chạy lần lượt qua từng Middleware từ trên xuống dưới (Inbound Phase).
   + Khi đến middleware cuối cùng (hoặc Endpoint), server xử lý logic và tạo ra HTTP Response.
   + Response sau đó chạy ngược từ dưới lên trên qua lại các middleware đó trước khi trả về cho client (Outbound Phase).
``` C#
Request  --->  [Middleware 1] ---> [Middleware 2] ---> [Endpoint / Controller]
                                                               |
Response <---  [Middleware 1] <--- [Middleware 2] <--- [Tạo kết quả]
```
- Các phương thức cấu hình middleware cốt lõi
    + App.Use(): Middleware chuyển tiếp </br>
        Thực thi code xử lý, sau đó gọi await next() để nhường quyền cho Middleware kế tiếp
```C#
app.Use(async (context, next) =>
{
    // Lượt đi (Inbound): Xử lý trước khi vào controller
    Console.WriteLine($"[LOG] Request path: {context.Request.Path}");

    await next(context); // Chuyển sang Middleware tiếp theo trong chuỗi [12]

    // Lượt về (Outbound): Xử lý sau khi Controller trả kết quả
    Console.WriteLine($"[LOG] Response Status Code: {context.Response.StatusCode}");
});
```
   + app.Run() — Terminal Middleware (Điểm dừng)</br>
 Đóng vai trò là chốt chặn cuối cùng của pipeline. Phương thức này không gọi next(), ngắt chuỗi pipeline và lập tức trả response về
 ```C#
 app.Run(async context =>
{
    await context.Response.WriteAsync("Hello World form Terminal Middleware!");
});
```
  + app.Map() / app.MapGroup() — Phân nhánh Pipeline </br>
Dùng để rẽ nhánh pipeline dựa trên đường dẫn URL
```C#
var todoGroup = app.MapGroup("/api/todos"); // [13]
todoGroup.MapGet("/", () => "Danh sách công việc");
```
  + Viết Custom Middleware Class</br>
Bạn có thể tách Middleware thành một file class riêng sử dụng RequestDelegate
```C#
public class CustomLogMiddleware
{
    private readonly RequestDelegate _next;

    public CustomLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) // [14]
    {
        // Logic xử lý Request
        await _next(context); // [14]
        // Logic xử lý Response
    }
}

// Khai báo trong Program.cs:
app.UseMiddleware<CustomLogMiddleware>(); // [14]
```
## III. Các middleware có trong file programs.cs
### 1. Nhóm Xử lý Lỗi, Chuẩn đoán & Ghi Log (Diagnostics & Logging)
- app.UseDeveloperExceptionPage(): Hiển thị trang báo lỗi chi tiết (Stack Trace) dành riêng cho môi trường phát triển (Development)
- app.UseExceptionHandler("/Error"): Bắt mọi ngoại lệ chưa được xử lý và chuyển hướng người dùng đến trang báo lỗi thân thiện ở môi trường Production
- app.UseStatusCodePages() / app.UseStatusCodePagesWithReExecute("/not-found"): Xử lý các mã trạng thái lỗi HTTP (như 404 Not Found) và hiển thị lại trang tương ứng
- app.UseHttpLogging(): Ghi log chi tiết về HTTP Request và Response (Headers, Query String, Duration, Body)
### 2. Nhóm Bảo mật & An toàn Mạng (Security & Transport)
- app.UseHsts(): Kích hoạt chuẩn bảo mật HSTS (HTTP Strict Transport Security) ép trình duyệt chỉ kết nối bằng HTTPS trên Production
- app.UseHttpsRedirection(): Tự động chuyển hướng toàn bộ luồng truy cập HTTP sang HTTPS
- app.UseForwardedHeaders(): Xử lý các HTTP Header chuyển tiếp (X-Forwarded-For, X-Forwarded-Proto) khi ứng dụng nằm đằng sau Proxy hoặc Load Balancer (Nginx, IIS, Azure)
- app.UseCookiePolicy(): Áp dụng các quy tắc quản lý Cookie (như SameSite, yêu cầu đồng ý Cookie)
### 3. Nhóm Tài nguyên Tĩnh (Static Assets)
- app.UseStaticFiles(): Cho phép server phục vụ các file tĩnh nằm trong thư mục wwwroot (.css, .js, hình ảnh)
- app.MapStaticAssets(): Phương thức tối ưu hóa việc phân phối tài nguyên tĩnh thế hệ mới (từ .NET 9) giúp tăng tốc độ tải file
- app.UseDefaultFiles(): Tự động tìm kiếm các file mặc định như index.html hoặc default.htm khi người dùng truy cập thư mục gốc
### 4. Nhóm Định tuyến, Tối ưu & Giới hạn (Routing, Rate Limiting & Optimization)
- app.UseRouting(): So khớp đường dẫn URL của Request với các Endpoint đã đăng ký trong hệ thống
- app.UseCors(): Áp dụng chính sách chia sẻ tài nguyên giữa các tên miền khác nhau (CORS - Cross-Origin Resource Sharing)
- app.UseRateLimiter(): Giới hạn số lượng Request/kết nối đồng thời gửi tới API trong một khoảng thời gian để chống tấn công Spam/DDoS
- app.UseResponseCompression(): Nén dữ liệu Response trả về client (dùng Gzip, Brotli hoặc Zstandard) để tiết kiệm băng thông
- app.UseRequestDecompression(): Tự động giải nén dữ liệu Request gửi lên từ client
- app.UseOutputCaching() / app.UseResponseCaching(): Bộ nhớ đệm lưu trữ nội dung Response phía server nhằm tối ưu thời gian phản hồi cho các request lặp lại
### 5. Nhóm Xác thực, Phân quyền & Phiên làm việc (Auth, CSRF & Session)
- app.UseAuthentication(): Xác thực danh tính người dùng (User là ai?)
- app.UseAuthorization(): Phân quyền truy cập (User có đủ quyền truy cập tài nguyên này không?)
- app.UseAntiforgery(): Bảo vệ ứng dụng khỏi các cuộc tấn công giả mạo yêu cầu chéo miền (CSRF/XSRF)
- app.UseSession(): Kích hoạt và quản lý trạng thái phiên làm việc (Session) giữa các HTTP Request
### 6. Nhóm Ánh xạ Endpoint (Endpoints & API Mapping)
- app.MapControllers(): Ánh xạ các Controller API / MVC trong dự án.
- app.MapControllerRoute(...): Định nghĩa cấu trúc route cho Web MVC ({controller=Home}/{action=Index}/{id?})
- app.MapRazorPages(): Ánh xạ các trang Razor Pages
- ```app.MapRazorComponents<App>()```: Định tuyến và kích hoạt các linh kiện giao diện Blazor
- ```app.MapHub<THub>("/path")```: Ánh xạ các cổng kết nối thời gian thực SignalR Hub
- app.MapGet(), app.MapPost(), app.MapGroup(): Định nghĩa các tuyến đường xử lý Minimal API
- app.MapHealthChecks("/health"): Cung cấp endpoint kiểm tra trạng thái hoạt động (Health Check) của ứng dụng
- app.UseSwagger() & app.UseSwaggerUI(): Mở trang tài liệu thử nghiệm API tương tác OpenAPI/Swagger
### 7. Các phương thức Tạo Custom Middleware
Ngoài các middleware tích hợp sẵn, bạn có thể tự viết middleware trực tiếp trong Program.cs
- app.Use(async (context, next) => { ... await next(); }): Chạy một đoạn mã tùy chỉnh và gọi next() để chuyển sang middleware tiếp theo
- app.Run(async context => { ... }): Điểm dừng cuối cùng (Terminal Middleware), xử lý và trả ngay kết quả mà không gọi tiếp các middleware sau
- app.Map("/api", appBranch => { ... }): Tách nhánh pipeline xử lý riêng cho các URL bắt đầu bằng đường dẫn chỉ định
- ```app.UseMiddleware<CustomMiddlewareClass>()```: Đăng ký một lớp Middleware tùy chỉnh được viết trong file .cs riêng vào pipeline
## IV. Thứ tự và ví dụ sử dụng toàn bộ middeware
```C#
var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// [ĐĂNG KÝ SERVICES VÀO DI CONTAINER (builder.Services)]
// =========================================================================
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký các dịch vụ bổ trợ
builder.Services.AddResponseCompression(); // Nén dữ liệu
builder.Services.AddCors();                // Chia sẻ tài nguyên khác tên miền
builder.Services.AddAuthentication();      // Xác thực
builder.Services.AddAuthorization();       // Phân quyền
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();             // Session
builder.Services.AddOutputCache();         // Caching

var app = builder.Build();

// =========================================================================
// NHÓM 1: XỬ LÝ LỖI, CHẨN ĐOÁN & GHI LOG (Diagnostics & Error Handling)
// Bắt buộc đặt ĐẦU TIÊN để bắt mọi ngoại lệ từ các middleware bên dưới
// =========================================================================
if (app.Environment.IsDevelopment())
{
    // Hiển thị trang báo lỗi chi tiết (Stack Trace) khi lập trình local
    app.UseDeveloperExceptionPage();
}
else
{
    // Bắt lỗi toàn cục và chuyển hướng đến trang báo lỗi chung trên Production
    app.UseExceptionHandler("/Home/Error");
    
    // Xử lý các mã trạng thái HTTP (như 404 Not Found)
    app.UseStatusCodePagesWithReExecute("/Home/NotFound/{0}");
}

// Ghi log chi tiết HTTP Request/Response (tùy chọn)
// app.UseHttpLogging();


// =========================================================================
// NHÓM 2: BẢO MẬT & KẾT NỐI MẠNG (Security & Transport)
// Bảo vệ kết nối mạng và ép sử dụng kênh truyền bảo mật
// =========================================================================
if (!app.Environment.IsDevelopment())
{
    // Ép trình duyệt chỉ kết nối bằng HTTPS (HSTS) trên Production
    app.UseHsts();
}

// Tự động chuyển hướng toàn bộ kết nối HTTP sang HTTPS
app.UseHttpsRedirection();

// Xử lý Header khi ứng dụng chạy sau Proxy/Load Balancer (Nginx, IIS)
// app.UseForwardedHeaders();


// =========================================================================
// NHÓM 3: TÀI NGUYÊN TĨNH (Static Assets / Static Files)
// Phục vụ file .css, .js, hình ảnh từ wwwroot mà không cần qua Routing/Auth
// =========================================================================
app.UseStaticFiles(); 
// Ngoài ra nếu không muốn đọc file tĩnh từ wwwroot mà một nơi khác thì cấu hình
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
    RequestPath = "/static" // URL truy cập sẽ là: https://localhost:port/static/filename.jpg
});
// Dành riêng cho .NET 9+ (Tối ưu hóa tải tài nguyên tĩnh)
// app.MapStaticAssets();


// =========================================================================
// NHÓM 4: ĐỊNH TUYẾN, NÉN & TỐI ƯU HÓA (Routing, Optimization & CORS)
// Phân tích URL và chuẩn bị điều kiện hạ tầng cho Request
// =========================================================================
// 1. Phân tích đường dẫn URL để xác định Endpoint phù hợp
app.UseRouting();

// 2. Nén dữ liệu Response trả về (Gzip/Brotli) để tiết kiệm băng thông
app.UseResponseCompression();

// 3. Chính sách CORS: BẮT BUỘC đặt SAU UseRouting() và TRƯỚC Auth
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Giới hạn số lượng Request/giây chống Spam/DDoS (tùy chọn)
// app.UseRateLimiter();


// =========================================================================
// NHÓM 5: XÁC THỰC & PHÂN QUYỀN (Authentication & Authorization)
// BẮT BUỘC: Authentication phải đứng TRƯỚC Authorization
// =========================================================================
// 1. Kiểm tra danh tính: User là ai? (Đọc Cookie, JWT Token)
app.UseAuthentication();

// 2. Kiểm tra quyền hạn: User có được phép vào tài nguyên này không?
app.UseAuthorization();


// =========================================================================
// NHÓM 6: PHIÊN LÀM VIỆC & BỘ NHỚ ĐỆM (Session & Caching)
// Lưu trữ dữ liệu tạm thời (Đặt sau Auth để gán Session đúng với User)
// =========================================================================
// Quản lý HttpContext.Session
app.UseSession();

// Bộ nhớ đệm lưu kết quả Response phía Server
app.UseOutputCache();


// =========================================================================
// NHÓM 7: ÁNH XẠ ENDPOINT & SWAGGER (Endpoints Mapping)
// Điểm dừng cuối cùng (Terminal) để thực thi hàm Controller / API
// =========================================================================
// 1. Cấu hình Swagger / OpenAPI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Bảo vệ đường dẫn Swagger (yêu cầu đăng nhập mới được xem)
// app.MapSwagger().RequireAuthorization();

// 2. Định tuyến cho MVC Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 3. Định tuyến cho Web API Controllers (dùng thuộc tính [Route])
app.MapControllers();

// 4. Định tuyến cho Razor Pages hoặc Blazor (nếu có)
// app.MapRazorPages();

// 5. Khởi chạy ứng dụng
app.Run();
```
Quy tắc "vàng" cần nhớ về thứ tự
+ UseExceptionHandler luôn đứng đầu: Để bắt được ngoại lệ văng ra từ tất cả middleware bên dưới.
+ UseStaticFiles đứng trước UseRouting: Để nếu request là file ảnh/script có sẵn trong wwwroot, server sẽ trả về file ngay lập tức mà không cần tốn tài nguyên chạy Routing hay Authentication.
+ UseRouting đứng trước UseCors và UseAuthorization: Giúp hệ thống biết chính xác request đang muốn truy cập vào Controller/Action nào trước khi kiểm tra quyền.
+ UseAuthentication luôn đứng trước UseAuthorization: Quy trình tự nhiên luôn là "Xác minh danh tính (Ai đây?)" rồi mới "Kiểm tra quyền hạn (Có được vào không?)".
+ Các hàm Map...() luôn ở cuối: Đóng vai trò là chốt chặn xử lý và trả kết quả về cho client.
Bảng tóm tắt nhanh

|Thứ tự|Nhóm Middleware|Lý do bắt buộc về vị trí|
|---|---|---|
|1|Xử lý Lỗi (Exception)|Đặt đầu tiên để bao bọc và bắt lỗi từ tất cả middleware chạy phía sau.|
|2|Bảo mật (HTTPS/HSTS)|Đảm bảo kênh truyền được mã hóa trước khi xử lý dữ liệu.|
|3|Static Files|Trả file tĩnh (.css, .js, ảnh) ngay lập tức mà không tốn tài nguyên chạy Routing hay Auth.|
|4|Routing & CORS|Phân tích URL trước, sau đó áp dụng chính sách CORS cho đúng route đó.|
|5|Auth (Authentication -> Authorization)|Bắt buộc Xác thực danh tính trước rồi mới Phân quyền truy cập.|
|6|Session & Caching|Đặt sau Auth để dữ liệu phiên làm việc được gắn đúng với User đã đăng nhập.|
|7|Endpoints (Map...)|Đặt cuối cùng để nhận request đã qua kiểm duyệt sạch sẽ và trả về dữ liệu.|
