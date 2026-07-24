# Xử lý lỗi khi ứng dụng gặp lỗi
Tạo các exception để tạo lỗi thân thiện, bắt lỗi những lỗi trả về lỗi 
thân thiện, chứ không phải một đống lỗi rối mù. Dễ đoán được ứng dụng đang
bị lỗi gì

Request  ──> [ Middleware 1 ] ──> [ Middleware 2 ] ──> [ Controller (NỔ LỖI 💥) ]
Response <── [ Middleware 1 ] <── [ Middleware 2 ] <── (Exception bị bắt tại đây)

Ví dụ sử dụng lỗi trong ASP .NET Core 
    
    Bước 1 cấu hình lớp xử lý lỗi 
    using Microsoft.AspNetCore.Diagnostics;

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Lỗi xảy ra: {Message}", exception.Message);

        var problemDetails = new
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = "Hệ thống gặp sự cố, chúng tôi đang khắc phục."
        };

        httpContext.Response.StatusCode = problemDetails.Status;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Trả về true nghĩa là Exception ĐÃ ĐƯỢC XỬ LÝ xong, không cần chuyền đi đâu nữa
        return true; 
    }}


    Bước 2 // Đăng ký Service
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails(); // Tạo chuẩn báo lỗi ProblemDetails (RFC 7807)

    var app = builder.Build();

    // Kích hoạt Middleware xử lý lỗi
    app.UseExceptionHandler();

    