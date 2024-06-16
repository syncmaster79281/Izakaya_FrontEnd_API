# Izakaya_FontEnd
居酒屋前台專案

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


## 資料庫設計
![image](https://github.com/syncmaster79281/Izakaya_BackEnd/assets/19486441/8919808d-3f4b-45a3-8a71-55d1ff231493)
![image](https://github.com/syncmaster79281/Izakaya_FontEnd_API/assets/19486441/717448bb-3d5d-4028-9309-bd44d7a1527f)
![image](https://github.com/syncmaster79281/Izakaya_FontEnd_API/assets/19486441/e12753f2-e59d-4bfe-a3bc-f88a5866cdc8)




## 金流參照
https://pay.line.me/tw/developers/apis/onlineApis?locale=zh_TW <br>
https://pay.line.me/tw/developers/apis/onlineApis?locale=zh_TW

