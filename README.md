# Izakaya_FontEnd
居酒屋前台API專案

## 專案介紹:
這是一個居酒屋居酒屋前台API(期末專題)

## 主要負責功能:
1. 串接第三方LinePay 金流
2. 實作API:
   - 點餐購物車
   - 座位API
   - 熱門產品API
   - 結帳API


## Api:
- LinePayController (Line 第三方金流串接)
- OrderPaymentsController (訂單API)
- OrdersController (熱門產品API)
- SeatCartsController  (點餐API)
- SeatsController (座位查詢API)

## 使用技術
1. Swagger
2. Asp.Net Core Web API
3. Entity Framework
4. HMAC-SHA256 加密
5. 使用FromQuery 接收多參數查詢
6. 撰寫IEnumerable 擴充方法 達成 自由選擇塞選條件


## 金流參照
https://pay.line.me/tw/developers/apis/onlineApis?locale=zh_TW <br>
https://pay.line.me/tw/developers/apis/onlineApis?locale=zh_TW

