namespace ISPAN.Izakaya.BLL_Service_.Dtos
{
    //Sandbox
    //https://sandbox-api-pay.line.me 整合測試用的測試環境。於web simulation page模擬LINE Pay App的付款交易

    //Hmac Signature
    //Algorithm : HMAC-SHA256
    //Key : Channel Secret （LINE Pay商家中心提供"Channel Id"和"Channel SecretKey"）
    //HTTP Method
    //GET : Channel Secret + URI + Query String + nonce
    //POST : Channel Secret + URI + Request Body + nonce

    //HTTP Method : POST
    //Signature = Base64(HMAC-SHA256((Your ChannelSecret + URI + RequestBody + nonce),Your ChannelSecret))

    //Your ChannelSecret 頻道密鑰
    //URI 要求的URI       (例如：/v3/payments/request)
    //RequestBody 要求的內容     訂單內容
    //nonce 隨機數



    //Common HTTP Request Header
    //Content-Type                        String  Y application/json
    //X-LINE-ChannelId                    String  Y 金流整合資訊 - Channel ID
    //X-LINE-Authorization-Nonce          String  Y UUID or timestamp(時間戳)
    //X-LINE-Authorization                String  Y HMAC Base64 簽章

    //Request API
    //POST /v3/payments/request

    // 一般付款 訂單內容
    //    {
    //    "amount" : 100,
    //    "currency" : "TWD",
    //    "orderId" : "MKSI_S_20180904_1000001",
    //    "packages" : [
    //    {
    //        "id" : "1",
    //        "amount": 100,
    //        "products" : [
    //        {
    //            "id" : "PEN-B-001",
    //            "name" : "Pen Brown",
    //            "imageUrl" : "https://pay-store.line.com/images/pen_brown.jpg",
    //            "quantity" : 2,
    //            "price" : 50
    //        }
    //        ]
    //    }
    //    ],
    //    "redirectUrls" : {
    //        "confirmUrl" : "https://pay-store.line.com/order/payment/authorize",
    //        "cancelUrl" : "https://pay-store.line.com/order/payment/cancel"
    //    }
    //}


    //透過此API，商家可向LINE Pay發送付款請求，並取得付款連結。
    //Confirm API
    //POST /v3/payments/{transactionId }/ confirm
    //body amount總金額 currency幣別   header一樣加密 



    public class LinePayOrderDetail
    {
        public int Amount { get; set; }
        public string Currency { get; set; } = "TWD";
        public string OrderId { get; set; }
        public List<Package> Packages { get; set; }
        public RedirectUrls RedirectUrls { get; set; }
    }

    public class Package
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public List<LinePayProduct> Products { get; set; }
    }

    public class LinePayProduct
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
    }

    public class RedirectUrls
    {
        public string ConfirmUrl { get; set; }
        public string CancelUrl { get; set; }
    }

}
