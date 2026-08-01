# JWT (Json Web Token)
## I. Khái niệm
Là một phương pháp nhỏ gọn và an toàn để truyền tải thông tin giữa các bên dưới dạng đối tượng JSON. Nó thường được sử dụng để xác thực và ủy quyền trong các ứng dụng web, cho phép người dùng truy cập các tài nguyên được bảo vệ mà không cần phải cung cấp thông tin đăng nhập nhiều lần. Các tính năng chính bao gồm:
+ Xác thực không trạng thái: Lưu trữ thông tin người dùng trong mã thông báo, giảm nhu cầu lưu trữ phiên trên máy chủ. (nghĩa là đối với stateful thì server phải lưu thông tin
của client trong session. Còn với JWT thì stateless nghĩa là không cần lưu gì, chỉ cần giải mã signature)
+ Trao đổi dữ liệu an toàn: Sử dụng chữ ký số để xác minh rằng mã thông báo không bị thay đổi.
+ Định dạng nhỏ gọn: Bao gồm ba phần—Tiêu đề, Nội dung và Chữ ký—được mã hóa thành một chuỗi duy nhất.
+ Được hỗ trợ rộng rãi: Hoạt động trên nhiều ngôn ngữ lập trình và nền tảng khác nhau.

## II. Cấu trúc
JWT cấu trúc gồm 3 thành phần HEADER.PAYLOAD.SIGNATURE

**Header:** <br/>
+ alg: Thuật toán được sử dụng để ký (ví dụ: HS256, RS256).
+ typ: Loại token, luôn là "JWT".

Sẽ tạo ra một mã ví dụ eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9

**Payload:** chứa thông tin về người dùng, còn được gọi là yêu cầu xác thực, và một số thông tin bổ sung bao gồm dấu thời gian phát hành và thời gian hết hạn của mã thông báo. <br/>

Các thông tin bổ sung

+ iss (Issuer): Xác định đơn vị phát hành token.
+ sub (Chủ thể): Đại diện cho người dùng hoặc thực thể mà mã thông báo này đề cập đến.
+ aud (Đối tượng): Chỉ định người nhận dự định.
+ exp (Thời hạn): Xác định thời điểm mã thông báo hết hạn.
+ iat (Issued At): Thời điểm mã thông báo được tạo.
+ nbf (Not Before) : Chỉ định thời điểm mã thông báo trở nên hợp lệ.

Sẽ tạo ta một mã ví dụ eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNzA4MzQ1MTIzLCJleHAiOjE3MDgzNTUxMjN9

**Signature**: Kết tợp header, payload và serectkey để tạo ra một mã kí tự

ví dụ: SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

Cuối cùng 3 mã trên kết hợp lại thành một token gửi cho client

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNzA4MzQ1MTIzLCJleHAiOjE3MDgzNTUxMjN9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c