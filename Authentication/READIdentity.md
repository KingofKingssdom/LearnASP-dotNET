# Lý thuyết Identity
## I. ASP.NET Core Identity là gì?
+ ASP.NET Core Identity là một hệ thống xác thực và ủy quyền mạnh mẽ và linh hoạt được tích hợp sẵn trong các ứng dụng ASP.NET Core
+ Nó cung cấp mọi thứ cần thiết để quản lý an toàn các tài khoản người dùng, vai trò, mật khẩu và quyền hạn mà không cần phải viết logic tùy chỉnh từ đầu
+ Bằng cách sử dụng ASP.NET Core Identity, các dev có thể dễ dàng thêm các tính năng thiết yếu như đăng ký, đăng nhập, khôi phục mật khẩu và đăng nhập bằng tài khoản mạng xã hội vào các ứng dụng web

## II. Các tính năng của APS .NET Core Identity
Quản lý người dùng:
+ Có thể dễ dàng tạo, cập nhật, xóa và khôi phục người dùng. Dữ liệu cơ bản của mỗi người dùng (như tên người dùng, email, số điện thoại) được quản lý thông qua một thực thể người dùng tập trung.
+ Các nhà phát triển có thể mở rộng thực thể này với các trường tùy chỉnh, chẳng hạn như ngày sinh, địa chỉ và các chi tiết liên quan khác.

Quản lý vai trò:
+ Vai trò là một cách để nhóm người dùng dựa trên quyền hạn hoặc trách nhiệm của họ (ví dụ: Quản trị viên, Người quản lý, Người điều hành, Khách hàng, v.v.).
+ Với ASP.NET Core Identity, bạn có thể tạo và quản lý các vai trò này, gán người dùng vào một hoặc nhiều vai trò, sau đó hạn chế hoặc cho phép truy cập vào các phần của ứng dụng dựa trên vai trò mà người dùng thuộc về.

Quản lý mật khẩu:
+ Mật khẩu được xử lý an toàn bằng cách băm (chuyển đổi mật khẩu thành định dạng mã hóa không thể đảo ngược).
+ Hệ thống định danh thực thi các chính sách về mật khẩu, bao gồm độ dài tối thiểu và yêu cầu về việc sử dụng chữ số/ký tự đặc biệt.
+ Nó cũng cung cấp các cơ chế cho người dùng thay đổi hoặc đặt lại mật khẩu của họ. Ngoài ra, nó còn hỗ trợ khôi phục mật khẩu thông qua các liên kết trong email.

Xác thực và ủy quyền người dùng:
+ Đây là quy trình xác minh danh tính người dùng khi họ cố gắng đăng nhập. ASP.NET Core Identity quản lý bước này để đảm bảo người dùng là hợp lệ.
+ Sau khi người dùng được xác thực, hệ thống sẽ xác định quyền hạn của họ dựa trên vai trò hoặc thông tin được gán (thông tin này là những chi tiết cụ thể về người dùng).

Xác thực dựa trên vai trò:
+ Điều này có nghĩa là kiểm soát quyền truy cập bằng cách gán vai trò cho người dùng, sau đó cho phép hoặc từ chối quyền truy cập vào các tài nguyên hoặc tính năng dựa trên các vai trò đó.

Xác thực dựa trên yêu cầu:
+ Ngoài vai trò, thông tin xác thực cung cấp khả năng kiểm soát chi tiết hơn. Ví dụ, thông tin xác thực có thể cho biết phòng ban, tuổi tác hoặc cấp độ đăng ký của người dùng, từ đó có thể được sử dụng để kiểm soát quyền truy cập vào một số tính năng nhất định.

Bảo mật tài khoản:
Các tính năng tích hợp giúp bảo vệ tài khoản:

+ Khóa tài khoản: Tạm thời vô hiệu hóa tài khoản sau quá nhiều lần đăng nhập không thành công.
+ Xác thực hai yếu tố (2FA): Bổ sung thêm một lớp bảo mật bằng cách yêu cầu người dùng xác minh danh tính của họ bằng thứ họ có (như mã ứng dụng di động hoặc tin nhắn SMS) ngoài thứ họ biết (mật khẩu).
+ Xác minh email/số điện thoại: Người dùng có thể được yêu cầu xác minh email hoặc số điện thoại trước khi có thể sử dụng đầy đủ tài khoản của mình.

Các nhà cung cấp đăng nhập bên ngoài:
+ Bạn có thể cho phép người dùng đăng nhập bằng tài khoản hiện có của họ từ các dịch vụ như Google hoặc Facebook, giúp quá trình đăng nhập dễ dàng và quen thuộc hơn với họ.

Khả năng tùy chỉnh:
+ Bạn có thể mở rộng các lớp người dùng và vai trò được tích hợp sẵn để thêm các trường riêng của mình, chẳng hạn như tên, họ, URL ảnh đại diện hoặc bất kỳ thông tin nào khác mà ứng dụng của bạn cần.

## Các thành phần chính của ASP.NET Core Identity
Ba lớp hoặc dịch vụ sau đây chịu trách nhiệm quản lý các thao tác khác nhau trên cơ sở dữ liệu định danh cho người dùng, cho phép chúng ta thực hiện các thao tác như tạo người dùng, gán vai trò và quản lý đăng nhập. Điều đó có nghĩa là, các lớp sau cung cấp nhiều phương thức mà chúng ta có thể sử dụng để thực hiện các thao tác khác nhau trên cơ sở dữ liệu định danh.

**Dịch vụ Quản lý Người dùng:**
UserManager là dịch vụ cốt lõi chịu trách nhiệm quản lý tài khoản người dùng trong hệ thống ASP.NET Core Identity. Nó đóng vai trò là điểm trung tâm cho tất cả các hoạt động liên quan đến người dùng, chẳng hạn như tạo người dùng mới, cập nhật thông tin người dùng, xóa người dùng và quản lý mật khẩu của họ. Nó đơn giản hóa sự phức tạp của việc xử lý dữ liệu người dùng một cách an toàn bằng cách:

+ Đảm bảo tên người dùng và địa chỉ email là duy nhất.
+ Thực thi các chính sách về độ mạnh và xác thực mật khẩu.
+ Mã hóa mật khẩu trước khi lưu trữ đảm bảo rằng mật khẩu dạng văn bản gốc sẽ không bao giờ được lưu lại.
+ Quản lý các tùy chọn khôi phục như mã đặt lại mật khẩu và mã xác nhận email.
+ Quản lý các tính năng bảo mật tài khoản, chẳng hạn như khóa tài khoản sau nhiều lần đăng nhập không thành công.

Tóm lại, UserManager bao hàm tất cả logic và các thực tiễn tốt nhất liên quan đến quản lý vòng đời người dùng, bảo mật và tính toàn vẹn dữ liệu.

**Dịch vụ Quản lý Vai trò**
RoleManager là dịch vụ chuyên dụng để quản lý các vai trò trong hệ thống định danh. Vai trò là một cách để nhóm người dùng và phân quyền một cách tập thể. RoleManager chịu trách nhiệm:

+ Tạo các vai trò mới (như “Quản trị viên”, “Người dùng” hoặc “Quản lý”).
+ Cập nhật hoặc xóa vai trò khi cần thiết.
+ Truy xuất thông tin vai trò cho mục đích quản trị hoặc ủy quyền.

Thông qua vai trò, các ứng dụng có thể triển khai kiểm soát truy cập dựa trên vai trò , nghĩa là quyền truy cập của người dùng vào các phần khác nhau của ứng dụng hoặc các chức năng được kiểm soát dựa trên vai trò được gán cho họ. RoleManager đơn giản hóa việc quản lý các vai trò này và vòng đời của chúng.

**Dịch vụ quản lý đăng nhập**
SignInManager tập trung vào khía cạnh xác thực, cụ thể là cách người dùng đăng nhập và đăng xuất khỏi ứng dụng. Các trách nhiệm của nó bao gồm:

+ Xác minh thông tin đăng nhập của người dùng (như tên người dùng và mật khẩu) trong quá trình đăng nhập.
+ Quản lý phiên xác thực hoặc cookie giúp người dùng duy trì trạng thái đăng nhập.
+ Việc triển khai các biện pháp bảo mật bổ sung, chẳng hạn như  xác thực hai yếu tố (2FA) , sẽ tăng cường khả năng bảo vệ.
Hỗ trợ đăng nhập thông qua các nhà cung cấp bên ngoài như tài khoản Google, Facebook hoặc Microsoft.
+ Quản lý quy trình đăng xuất để chấm dứt phiên làm việc của người dùng một cách an toàn.

SignInManager quản lý toàn bộ quy trình đăng nhập và đảm bảo rằng chỉ những người dùng hợp lệ, đã được xác thực mới có quyền truy cập vào hệ thống.

## Các model trong ASP.NET Core Identity

Hiểu rõ các thực thể này là chìa khóa để thấy được cách ASP.NET Core Identity tổ chức và bảo mật dữ liệu người dùng và vai trò bên trong. Sau đây là các mô hình thực thể được sử dụng trong ASP.NET Core Identity. Cùng nhau, các mô hình sau đây đại diện cho cấu trúc quan hệ hỗ trợ một hệ thống xác thực và ủy quyền người dùng linh hoạt, an toàn và có khả năng mở rộng trong ASP.NET Core Identity.

**IdentityUser Model**
IdentityUser là mô hình dữ liệu cơ bản đại diện cho người dùng trong hệ thống Identity của ASP.NET Core. Nó lưu trữ tất cả thông tin cốt lõi về người dùng, chẳng hạn như:

+ Tên người dùng
+ Địa chỉ email
+ Số điện thoại
+ Mã băm mật khẩu (mật khẩu đã được mã hóa)
+ Chi tiết về việc khóa tài khoản (để ngăn chặn các cuộc tấn công vét cạn mật khẩu)
+ Các dữ liệu khác liên quan đến bảo mật và hồ sơ

Mô hình này đóng vai trò là lớp cơ sở cho người dùng và có thể được mở rộng với các thuộc tính bổ sung (như Tên, Họ, Ngày sinh, Lần đăng nhập cuối, v.v.) để phù hợp với nhu cầu cụ thể của ứng dụng. Về cơ bản, mỗi người đăng ký hoặc được nhận dạng trong ứng dụng của bạn đều tương ứng với một bản ghi IdentityUser.

**IdentityRole Model**
IdentityRole đại diện cho một vai trò trong ứng dụng. Vai trò là các nhóm hoặc danh mục logic của người dùng, chẳng hạn như “Quản trị viên”, “Người điều hành”, “Khách hàng” hoặc “Quản lý”. Mô hình này lưu trữ thông tin cơ bản về vai trò, bao gồm:
+ Tên vai trò
+ Mã định danh vai trò (ID)
+ Bạn cũng có thể tùy chọn mở rộng để thêm mô tả hoặc siêu dữ liệu về vai trò đó.

Vai trò giúp thực thi kiểm soát truy cập dựa trên vai trò (RBAC) bằng cách nhóm các quyền và sau đó gán các vai trò đó cho người dùng.

**IdentityUserRole Model**
IdentityUserRole thể hiện mối quan hệ nhiều-nhiều giữa người dùng và vai trò. Vì một người dùng có thể có nhiều vai trò và một vai trò có thể được gán cho nhiều người dùng, mô hình này hoạt động như một bảng liên kết, kết nối người dùng với vai trò của họ. Nó lưu trữ:

+ ID người dùng
+ ID vai trò

Mối quan hệ này cho phép ASP.NET Core Identity nhanh chóng xác định người dùng cụ thể thuộc vai trò nào.

**IdentityUserLogin Model**
Mô hình IdentityUserLogin lưu trữ thông tin về các nhà cung cấp đăng nhập bên ngoài được liên kết với tài khoản người dùng. Khi người dùng đăng nhập bằng các dịch vụ của bên thứ ba như Google, Facebook hoặc Microsoft, mô hình này sẽ theo dõi:

+ Tên của nhà cung cấp đăng nhập bên ngoài (ví dụ: “Google”, “Facebook”, v.v.)
+ Khóa nhà cung cấp duy nhất hoặc ID người dùng từ nhà cung cấp bên ngoài
+ Người dùng trong hệ thống của bạn mà thông tin đăng nhập bên ngoài này thuộc về

Điều này cho phép xác thực từ bên ngoài và liên kết nhiều tài khoản đăng nhập bên ngoài với một tài khoản người dùng cục bộ duy nhất.

**IdentityUserClaim Model**
IdentityUserClaim đại diện cho một yêu cầu (claim) được liên kết với một người dùng cụ thể. Yêu cầu là một mẩu thông tin về người dùng, chẳng hạn như tuổi, địa chỉ email, cấp độ thành viên hoặc quyền hạn của họ. Yêu cầu cho phép kiểm soát chi tiết hơn so với chỉ dựa trên vai trò. Mô hình này lưu trữ:

+ Loại yêu cầu bồi thường (ví dụ: “Phòng ban”, “Tuổi”, “Cấp độ đăng ký”)
+ Giá trị yêu cầu bồi thường (ví dụ: “HR”, “30”, “Vàng”)
Người dùng mà yêu cầu bồi thường thuộc về

Các yêu cầu rất hữu ích cho việc ủy ​​quyền dựa trên yêu cầu , trong đó các quyết định truy cập phụ thuộc vào các thuộc tính người dùng này

**IdentityRoleClaim Model**
IdentityRoleClaim đại diện cho các yêu cầu được gán cho một vai trò chứ không phải cho từng người dùng riêng lẻ. Thay vì gán yêu cầu cho từng người dùng một, bạn có thể gán chúng cho các vai trò. Sau đó, tất cả người dùng trong vai trò đó sẽ kế thừa các yêu cầu này. Điều này hữu ích khi bạn muốn các yêu cầu dựa trên vai trò, trong đó quyền hoặc thuộc tính được cấp cho toàn bộ nhóm vai trò. Nó lưu trữ:

+ ID vai trò
+ Loại yêu cầu bồi thường và giá trị yêu cầu bồi thường liên quan đến vai trò đó

**IdentityUserToken Model**
IdentityUserToken lưu trữ các mã thông báo liên quan đến người dùng được sử dụng cho mục đích bảo mật. Các mã thông báo có thể là:

+ Mã đặt lại mật khẩu
+ Mã xác thực cho xác thực hai yếu tố
+ Mã thông báo làm mới cho đăng nhập liên tục

Các mã thông báo này được lưu trữ an toàn và liên kết với người dùng để giúp quản lý quy trình xác thực và các hoạt động bảo mật yêu cầu mã thông báo tạm thời hoặc có thể thu hồi.

**IdentityDbContext**
IdentityDbContext là một lớp ngữ cảnh cơ sở dữ liệu đặc biệt của Entity Framework Core được thiết kế để xử lý tất cả dữ liệu liên quan đến định danh trong một ứng dụng ASP.NET Core. Hãy coi nó như một trình quản lý trung tâm kết nối thông tin người dùng và vai trò của bạn với cơ sở dữ liệu.

Nó định nghĩa các bảng, mối quan hệ và cấu trúc cơ sở dữ liệu cần thiết cho hệ thống Định danh. Sử dụng IdentityDbContext, bạn có thể dễ dàng tương tác với dữ liệu định danh, chẳng hạn như người dùng, vai trò, yêu cầu và mã thông báo, thông qua Entity Framework Core.

Hãy tưởng tượng ứng dụng của bạn như một khách sạn, và IdentityDbContext là người quản lý lễ tân. Bất cứ khi nào bạn cần đăng ký khách, phân phòng, quản lý chìa khóa hoặc làm thủ tục trả phòng, người quản lý đều biết cách cập nhật tất cả các bản ghi (bảng cơ sở dữ liệu) cần thiết một cách hiệu quả.

**Tại sao IdentityDbContext lại quan trọng?**
+ Bảng định danh được xác định trước: Nó cung cấp các thuộc tính DbSet cho tất cả các bảng định danh tiêu chuẩn, chẳng hạn như người dùng (IdentityUser), vai trò (IdentityRole) và mối quan hệ giữa chúng (IdentityUserRole).
+ Quản lý lược đồ cơ sở dữ liệu: Khi bạn áp dụng các migration của Entity Framework, IdentityDbContext sẽ tạo tất cả các bảng cần thiết như AspNetUsers, AspNetRoles và AspNetUserRoles.
+ Có thể tùy chỉnh: Bạn có thể mở rộng IdentityDbContext để thêm các trường hoặc mối quan hệ tùy chỉnh cho người dùng và vai trò. Bạn cũng có thể thêm các bảng bổ sung (DbSets) để hỗ trợ các nhu cầu cụ thể của ứng dụng.

## Cách IdentityDbContext hoạt động trong ứng dụng
Khi chúng ta thực hiện các thao tác như tạo người dùng, gán vai trò hoặc xác thực thông tin đăng nhập, các dịch vụ ASP.NET Core Identity sẽ sử dụng IdentityDbContext nội bộ để lưu trữ và truy xuất dữ liệu.

IdentityDbContext trừu tượng hóa các chi tiết của cơ sở dữ liệu và cho phép bạn làm việc với dữ liệu định danh bằng Entity Framework Core.

Bạn có thể cấu hình IdentityDbContext với chuỗi kết nối cơ sở dữ liệu, các migration và các tùy chọn EF Core khác.

## IdentityDbContext hoạt động như thế nào trong ứng dụng của bạn?
Khi ứng dụng của bạn tạo người dùng, gán vai trò hoặc kiểm tra thông tin xác thực, các dịch vụ Định danh sẽ sử dụng IdentityDbContext ở phía sau để lưu và truy xuất thông tin này từ cơ sở dữ liệu.

Nó che giấu sự phức tạp của việc truy cập cơ sở dữ liệu thô, cho phép bạn dễ dàng làm việc với dữ liệu định danh bằng cách sử dụng các lớp dịch vụ Định danh.

Do đó, ASP.NET Core Identity giúp đơn giản hóa quy trình triển khai xác thực và ủy quyền người dùng an toàn trong các ứng dụng web hiện đại. Với các mô hình có thể tùy chỉnh, dịch vụ quản lý toàn diện và hỗ trợ cả nhà cung cấp đăng nhập cục bộ và bên ngoài, nó cho phép các nhà phát triển xây dựng hệ thống quản lý người dùng giàu tính năng, an toàn và có khả năng mở rộng một cách hiệu quả.