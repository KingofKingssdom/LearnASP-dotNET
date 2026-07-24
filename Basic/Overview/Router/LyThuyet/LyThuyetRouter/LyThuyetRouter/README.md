# Routing trong ASP.NET Core

## I. Khái niệm
Routing là định tuyến chịu trách nhiệm khớp các yêu cầu HTTP đến và chuyển tiếp các yêu cầu đó 
đến các điểm cuối (endpoint) có thể thực thi của ứng dụng

Hiểu kĩ thêm nữa là Routing" (Định tuyến) Là HÀNH ĐỘNG/CƠ CHẾ lấy cái Path đó đem đi so sánh (match) với các Controller/Action/Endpoint mà 
đã khai báo trong code, nhằm tìm ra nơi xử lý thích hợp.
## II. Nguyên tắc hoạt động

Qua ví dụ có url = https://localhost:7174/product/shore/1

Client gửi request: https://localhost:7174/product/shore/1

+ Kestrel nhận request, gọt giũa lấy ra Path: /product/shore/1

+ Routing Engine nhảy vào làm việc (Hành động Routing):

	+ Nó quét qua danh sách các Endpoint bạn đã định nghĩa.

	+ Nó thấy mẫu product/shore/{id} khớp hoàn toàn với đường dẫn /product/shore/1.

+ Nó bóc tách ra luôn: id = 1.

+ Kết quả của Routing: Nó đưa Request đi đúng vào hàm GetProductById(int id) để thực thi.

