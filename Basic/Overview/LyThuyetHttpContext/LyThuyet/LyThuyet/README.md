# Lý thuyết 
## I. Khái niệm
Hiểu một cách đơn giản nhất, HttpContext là một đối tượng chứa 
toàn bộ thông tin về một yêu cầu HTTP (HTTP Request) gửi đến server 
và phản hồi (HTTP Response) tương ứng mà server sẽ gửi lại cho client.

Mỗi khi có một request gửi tới ứng dụng ASP.NET Core, Web Server 
(như Kestrel) sẽ tạo ra đúng một phiên bản HttpContext đại diện 
cho lượt giao dịch đó.

## II. Các thuộc tính cần nhớ
HttpContext là một class trừu tượng (abstract class) đóng vai trò là điểm 
truy cập trung tâm. Dưới đây là các thuộc tính
### 1. HttpRequest
HttpContext.Request cung cấp quyền truy cập vào HttpRequest. Nó chứa thông 
tin về yêu cầu HTTP đến và được khởi tạo khi máy chủ nhận được yêu cầu HTTP. 
Nó chứa thông tin về yêu cầu HTTP đến và được khởi tạo khi máy chủ nhận được yêu cầu HTTP

**a. HttpRequest.Path**

Trả về đường dẫn URL của request hiện tại (không bao gồm Domain và Query String).

Ví dụ: xem trong thư mục ViDuHttpRequest/Path 

**b. HttpRequest.Method**

Trả về phương thức của request hiện tại

Ví dụ: Xem trong thư mục ViduHttpContext/method

**c. Http.Request.Headers**

Trả về Header của request gửi tới

Ví dụ: Xem trong thư mục ViduHttpContext/Header

**d. HttpRequest.RouteValue**

Trả về các tham số url phía sau với url/{param1}/{param2}
nó là một tập hợp (dictionary) trả ra key - value 

Ví dụ: Xem trong thư mục ViduHttpContext/RouteValue

**e. HttpRequest.Query**

Trả về các tham số url phía sau với url?key={param1}&key2={param2}

Ví dụ: Xem trong thư mục ViduHttpContext/Query

**f. HttpRequest.ReadFormAsync**

Trả về các giá trị khi request gửi đến là một form. Nền dùng
trong request với giá trị gửi lên là form data

ví dụ: 

**g. HttpRequest.Body**

Trả về dữ liệu trong request body nhưng mà nó là dữ liệu
dạng bit 0 1 (stream) 

### 2. HttpResponse
HttpContext.Response cung cấp quyền truy cập vào HttpResponse và được sử dụng để thiết lập thông tin về phản hồi HTTP được gửi lại cho máy khách.

**a. HttpResponse.StatusCode**

Trả về mã trạng thái phản hồi

**b. HttpResponse.ContentType**

Tiêu đề phản hồi content-type. Phải được thiết lập trước khi ghi vào phần thân phản hồi.

**c. HttpResponse.Headers**

Một tập hợp các tiêu đề phản hồi. Phải được thiết lập trước khi ghi vào phần thân phản hồi.

**d. HttpResponse.Body**

Trả về nôi dung Response nhưng dưới dạng stream 0 1

### 3. HttpContext không an toàn cho đa luồng

