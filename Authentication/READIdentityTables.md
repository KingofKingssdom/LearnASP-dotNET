# Các tables trong Identity ASP .NET Core
## 1. Bảng AspNetUsers
Bảng AspNetUsers lưu trữ tất cả thông tin cần thiết về người dùng ứng dụng của bạn. Nó chứa thông tin nhận dạng duy nhất, chi tiết đăng nhập, cài đặt bảo mật và thông tin liên hệ của người dùng. Bảng này đóng vai trò là nguồn dữ liệu chính liên quan đến người dùng trong hệ thống Identity

Các cột trong bảng AspNetUsers thường bao gồm những mục sau:
+ Id : Mã định danh duy nhất cho mỗi người dùng (khóa chính).
+ UserName : Tên đăng nhập của người dùng.
+ NormalizedUserName : Phiên bản viết hoa của UserName để tìm kiếm không phân biệt chữ hoa chữ thường.
+ Email : Địa chỉ email của người dùng.
+ NormalizedEmail : Phiên bản viết hoa của Email để tra cứu không phân biệt chữ hoa chữ thường.
+ EmailConfirmed : Cho biết email của người dùng đã được xác minh hay chưa.
+ PasswordHash : Dạng mã hóa của mật khẩu người dùng (không bao giờ được lưu trữ dưới dạng văn bản thuần).
+ SecurityStamp:  Một giá trị ngẫu nhiên cho biết liệu thông tin người dùng liên quan đến bảo mật có thay đổi hay không. Ví dụ, giá trị này thay đổi khi người dùng thay đổi mật khẩu, đặt lại mật khẩu hoặc thêm thông tin đăng nhập bên ngoài. Mục đích chính của nó là vô hiệu hóa bất kỳ phiên hoặc cookie hiện có nào khi thông tin liên quan đến bảo mật thay đổi. Đây là một biện pháp bảo mật để đảm bảo rằng các phiên cũ không còn hiệu lực nếu thông tin đăng nhập bị xâm phạm và sau đó bị thay đổi.
+ ConcurrencyStamp: Một giá trị duy nhất thay đổi mỗi khi hồ sơ người dùng được cập nhật, được sử dụng để kiểm soát đồng thời lạc quan, đảm bảo tính toàn vẹn dữ liệu. Trong các trường hợp có nhiều nỗ lực sửa đổi cùng một bản ghi người dùng đồng thời, ConcurrencyStamp đảm bảo rằng các thay đổi không xung đột.
+ Phone : Số điện thoại của người dùng.
+ PhoneNumberConfirmed : Cho biết số điện thoại đã được xác minh hay chưa.
+ TwoFactorEnabled : Cho biết xác thực hai yếu tố có đang được kích hoạt hay không.
+ LockoutEnd : Ngày/giờ kết thúc thời gian khóa tài khoản sau các lần đăng nhập không thành công.
+ LockoutEnabled : Cho biết tính năng khóa tài khoản có được bật cho người dùng hay không.
+ AccessFailedCount : Đếm số lần đăng nhập không thành công để kích hoạt khóa tài khoản nếu cần thiết. 

**Mối quan hệ:** Bảng AspNetUsers có liên hệ với hầu hết các bảng Identity khác. Mỗi người dùng có thể có nhiều vai trò (thông qua AspNetUserRoles), nhiều quyền truy cập (AspNetUserClaims), nhiều tài khoản đăng nhập bên ngoài (AspNetUserLogins) và nhiều mã thông báo (AspNetUserTokens). Các mối quan hệ này sử dụng cột Id làm khóa ngoại trong các bảng liên quan.
## 2. Bảng AspNetRoles
Bảng AspNetRoles chứa tất cả các vai trò được định nghĩa cho ứng dụng. Vai trò đại diện cho các nhóm hoặc cấp độ quyền khác nhau (ví dụ: Quản trị viên, Người điều hành, Người dùng) có thể được gán cho người dùng

Các cột trong bảng AspNetRoles thường bao gồm những mục sau:
+ Id: Mã định danh duy nhất cho mỗi vai trò (thường là GUID).
+ Name: Tên dễ đọc của vai trò (ví dụ: “Quản trị viên”).
+ NormalizedName: Phiên bản viết hoa của tên vai trò để so khớp không phân biệt chữ hoa chữ thường.
+ ConcurrencyStamp: Giá trị thay đổi khi có cập nhật, được sử dụng để kiểm soát đồng thời.

**Mối quan hệ:** Bảng AspNetRoles liên kết với người dùng thông qua bảng AspNetUserRoles (gán vai trò cho người dùng) và với các yêu cầu thông qua bảng AspNetRoleClaims (gán yêu cầu/quyền hạn cho các vai trò).

## 3. Bảng AspNetUserRoles
Bảng AspNetUserRoles thể hiện mối quan hệ nhiều-nhiều giữa người dùng và vai trò. Nó liên kết người dùng với các vai trò tương ứng của họ, cho phép ủy quyền dựa trên vai trò

Các cột trong bảng AspNetUserRoles thường bao gồm những mục sau:
+ UserId:  Mã định danh người dùng từ bảng AspNetUsers. Là một phần của khóa chính phức hợp. Nó tương ứng với cột Id trong bảng AspNetUsers. Nó hoạt động như một khóa ngoại liên kết đến bảng AspNetUsers.
+ RoleId:  Mã định danh vai trò từ bảng AspNetRoles. Là một phần của khóa chính phức hợp. Nó tương ứng với cột Id trong bảng AspNetRoles. Nó hoạt động như một khóa ngoại liên kết đến bảng AspNetRoles.

**Mối quan hệ:** Bảng này kết nối AspNetUsers và AspNetRoles, cho phép tra cứu tất cả các vai trò của một người dùng cụ thể hoặc tất cả người dùng cho một vai trò cụ thể.

## 4. Bảng AspNetUserLogins
Bảng AspNetUserLogins lưu trữ thông tin về các nhà cung cấp xác thực bên ngoài mà người dùng sử dụng để đăng nhập, chẳng hạn như Google hoặc Facebook. Nó liên kết các thông tin đăng nhập bên ngoài này với các tài khoản người dùng cục bộ

Các cột trong bảng AspNetUserLogins thường bao gồm những mục sau:

+ LoginProvider : Cột này lưu trữ tên của nhà cung cấp xác thực bên ngoài (ví dụ: Google, Facebook, Microsoft, v.v.). Nó là một phần của khóa chính phức hợp cho bảng.
+ ProviderKey : Cột này lưu trữ mã định danh duy nhất do nhà cung cấp đăng nhập bên ngoài cung cấp cho người dùng. Ví dụ, khi người dùng đăng nhập bằng Google, trường này sẽ lưu trữ ID duy nhất được Google gán cho người dùng. Nó cũng là một phần của khóa chính phức hợp.
+ ProviderDisplayName : Cột tùy chọn này có thể lưu trữ tên hiển thị của nhà cung cấp đăng nhập (ví dụ: “Google” thay vì “google.com”). Nó chủ yếu được sử dụng cho mục đích hiển thị trong giao diện người dùng.
+ UserId : Mã định danh người dùng cục bộ được liên kết với tài khoản đăng nhập này. Nó hoạt động như một khóa ngoại liên kết đến cột Id trong bảng AspNetUsers.

**Mối quan hệ:** Mỗi mục trong bảng này liên kết một người dùng (UserId) với một nhà cung cấp xác thực bên ngoài, cho phép người dùng đăng nhập bằng tài khoản mạng xã hội.

## 5. Bảng AspNetUserTokens
Bảng AspNetUserTokens lưu trữ các mã thông báo cho người dùng, có thể được sử dụng cho nhiều mục đích khác nhau, chẳng hạn như đặt lại mật khẩu, xác nhận email hoặc xác thực hai yếu tố

Các cột trong bảng AspNetUserTokens thường bao gồm những cột sau:

+ UserId:  Mã định danh người dùng từ bảng AspNetUsers.
+ LoginProvider:  Tên của nhà cung cấp đã tạo ra mã thông báo.
+ Name:  Cột này lưu trữ tên của mã thông báo, chẳng hạn như mã thông báo xác nhận email, mã thông báo đặt lại mật khẩu hoặc mã thông báo xác thực hai yếu tố. Nó có thể là PasswordReset, EmailConfirmation, AccessToken, v.v.
+ Value:  Giá trị của token. 

**Mối quan hệ:** Mỗi mục nhập mã thông báo được liên kết với một người dùng và thường được sử dụng cho các hoạt động bảo mật hoặc các bước quy trình công việc yêu cầu mã thông báo dùng một lần.

## 6. Bảng AspNetUserClaims
Bảng AspNetUserClaims lưu trữ các thông tin bổ sung liên quan đến từng người dùng. Thông tin xác thực là các cặp khóa-giá trị chứa thông tin bổ sung về người dùng, thường được sử dụng trong quá trình xác thực. Mỗi thông tin xác thực là một cặp khóa-giá trị được liên kết với một người dùng và có thể đại diện cho nhiều thuộc tính khác nhau như địa chỉ email, tên đầy đủ, vai trò hoặc bất kỳ dữ liệu tùy chỉnh nào khác liên quan đến ứng dụng

Các cột trong bảng AspNetUserClaims thường bao gồm những mục sau:

+ Id:  Khóa chính cho thông tin người dùng. Đây là một số nguyên.
+ UserId:  Mã định danh của người dùng được liên kết với yêu cầu này. Nó đóng vai trò là khóa ngoại liên kết đến cột Id trong bảng AspNetUsers.
+ ClaimType:  Loại yêu cầu bồi thường (ví dụ: “ngày sinh”).
+ ClaimValue:  Giá trị của yêu cầu bồi thường (ví dụ: “1980-01-01”).

**Mối quan hệ:** Mỗi mục yêu cầu liên kết với một người dùng và xác định các thuộc tính hoặc quyền truy cập bổ sung dành riêng cho người dùng đó.

## 7. Bảng AspNetRoleClaims
Bảng AspNetRoleClaims lưu trữ các yêu cầu liên quan đến vai trò. Các yêu cầu này áp dụng cho tất cả người dùng được gán vai trò đó, cho phép các yêu cầu dựa trên quyền ở cấp độ vai trò

Các cột trong bảng AspNetRoleClaims thường bao gồm những cột sau:
+ Id:  Mã định danh số nguyên duy nhất cho yêu cầu vai trò.
+ RoleId:  Mã định danh vai trò liên kết với yêu cầu này. Đây là khóa ngoại liên kết đến cột Id trong bảng AspNetRoles. 
+ ClaimType:  Cột này lưu trữ loại yêu cầu, chẳng hạn như Permission, AccessLevel hoặc bất kỳ loại nào khác phù hợp với ngữ cảnh ứng dụng của bạn.
+ ClaimValue:  Giá trị thực tế của yêu cầu. Ví dụ, nếu loại yêu cầu là Quyền , giá trị yêu cầu có thể là Chỉnh sửa người dùng hoặc Xem báo cáo , v.v.

**Mối quan hệ:** Mỗi mục yêu cầu được liên kết với một vai trò cụ thể. Tất cả người dùng trong vai trò đó sẽ kế thừa các yêu cầu được định nghĩa ở đây.

**Lưu ý:**  Cấu trúc của các bảng nhất quán trên các nhà cung cấp cơ sở dữ liệu khác nhau (ví dụ: SQL Server, PostgreSQL, MySQL), nhưng có thể có những khác biệt nhỏ về kiểu dữ liệu hoặc ràng buộc tùy thuộc vào hệ thống cơ sở dữ liệu được sử dụng.

