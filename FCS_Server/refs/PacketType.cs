using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public class PacketType
    {
        public const byte HEADER = 0xFF;
        public const byte Initialize = 0x01; //Front Channel Service¿¡ ¿¬°á     
        public const byte KeepAlive = 0x02; //¿¬°á»óÅÂ È®ÀÎ                    
        // Authentication & Authorization
        public const byte GameLogin = 0x11; //°ÔÀÓ·Î±×ÀÎ                       
        public const byte GameLogout = 0x12; //°ÔÀÓ·Î±×¾Æ¿ô                     
        public const byte PremiumServiceExpiredCallback = 0x13; //ÇÁ¸®¹Ì¾ö ¼­ºñ½º ±â°£ ¸¸·á Åëº¸   
        public const byte ForceUserLogoutCallback = 0x14; //»ç¿ëÀÚ °­Á¦ ·Î±×¾Æ¿ô             
        public const byte RequestAuthenticationKey = 0x21; //ÀÎÁõÅ° ¿äÃ»                      
        public const byte ValidateAuthenticationKey = 0x22; //ÀÎÁõÅ° À¯È¿¼º È®ÀÎ               
        public const byte ValidateAuthenticationKeyWithExtension = 0x23; //ÀÎÁõÅ° À¯È¿¼º È®ÀÎ               
        public const byte ValidateAuthenticationKeyForPublisher = 0x24; //ÀÎÁõÅ° À¯È¿¼º È®ÀÎ
        public const byte ValidateAuthenticationKeyWithParentSafe = 0x25;  //ºÎ¸ð ¾È½É ¼­ºñ½º ÀÎÁõÅ° À¯È¿¼º °Ë»ç
        public const byte ValidateAuthenticationKeyForR2 = 0x26; //±¹³» R2 ¿ë ÀÎÁõÅ° À¯È¿¼º °Ë»ç
        public const byte ValidateAuthenticationKeyWithUserInfo = 0x27; //ÀÎÁõÅ° À¯È¿¼º È®ÀÎ(À¯Àú Á¤º¸ Á¶È¸)
        public const byte AccountLogin = 0x31; //WEBZEN Account Login
        public const byte AccountLogout = 0x32; //WEBZEN Account Logout
        public const byte SetAccountState = 0x33; //°èÁ¤ÀÇ ¸¶Áö¸· ·Î±×ÀÎ/·Î±×¾Æ¿ô Á¤º¸ ¾÷µ¥ÀÌÆ®
        public const byte GetPCRoomGuid = 0x34; //IP·Î PC¹æ °íÀ¯¹øÈ£ ¹ÝÈ¯
        public const byte CheckPhoneAuthUser = 0x35; //ÀüÈ­ÀÎÁõ °¡ÀÔ°èÁ¤ ¿©ºÎ È®ÀÎ
        public const byte GetUserInfo = 0x36; //°èÁ¤¹øÈ£·Î »ç¿ëÀÚ Á¤º¸ È®ÀÎ
        public const byte GetUserInfoWithExtension = 0x37; //°èÁ¤¹øÈ£·Î »ç¿ëÀÚ Á¤º¸ È®ÀÎ
        public const byte ActivateFlatRatePaymentProduct = 0x41; //Á¤¾×»óÇ° È°¼ºÈ­                  
        public const byte DeactivateFlatRatePaymentProduct = 0x42; //Á¤¾×»óÇ° ºñÈ°¼ºÈ­                
        // Basic Billing
        public const byte CheckBalance = 0x51; //ÀÜ¾×Á¶È¸                         
        public const byte ItemPurchase = 0x61; //º¹¼ö ¾ÆÀÌÅÛ ±¸¸Å                 
        public const byte ItemGift = 0x62; //º¹¼ö ¾ÆÀÌÅÛ ¼±¹°                 
        public const byte CheckPurchase = 0x63; //±¸¸ÅÈ®ÀÎ                         
        public const byte CancelPurchase = 0x64; //±¸¸ÅÃë¼Ò                         
        public const byte CancelPurchaseByOrderId = 0x65; //ÁÖ¹®¹øÈ£¸¦ ÀÌ¿ëÇØ¼­ ÀüÃ¼ ±¸¸ÅÃë¼Ò
        public const byte ConfirmPurchaseByOrderId = 0x66; //ÁÖ¹®¹øÈ£¸¦ ÀÌ¿ëÇØ¼­ ÀüÃ¼ ±¸¸Å½ÂÀÎ
        public const byte PurchaseList = 0x71; //±¸¸Å³»¿ªÁ¶È¸
        public const byte ExchangeWCoin = 0x72; //WCoinÀ» °ÔÀÓ ³»¿¡¼­ »ç¿ëµÇ´Â Æ÷ÀÎÆ®·Î ÀüÈ¯
        // WShop Billing
        public const byte WShopCheckBalance = 0x90; //ÀÜ¾×Á¶È¸
        public const byte WShopPurchase = 0x91; //¼¥ ¾ÆÀÌÅÛ ±¸¸Å
        public const byte WShopCheckPurchase = 0x92; //¼¥ ¾ÆÀÌÅÛ ±¸¸Å È®ÀÎ
        public const byte WShopCancelPurchase = 0x93; //¼¥ ¾ÆÀÌÅÛ ±¸¸Å Ãë¼Ò
        public const byte WShopConfirmPurchase = 0x94; //¼¥ ¾ÆÀÌÅÛ ±¸¸Å ½ÂÀÎ
        public const byte WShopGift = 0x95; //¼¥ ¾ÆÀÌÅÛ ¼±¹°
        public const byte WShopCheckGift = 0x96; //¼±¹° °Ç È®ÀÎ
        public const byte WShopCancelGift = 0x97; //¼±¹° °Ç Ãë¼Ò
        public const byte WShopConfirmGift = 0x98; //Áê¾ó ÃæÀü °Ç ½ÂÀÎ
        public const byte WShopGetVersion = 0x99; //¼¥ ¹öÀü Á¤º¸ Á¶È¸
        // WShop Inventory
        public const byte WShopInquiryNewArrival = 0x9A; //ÀÏÀÚ ±âÁØÀ¸·Î »õ·Î µé¾î¿Â ÀÎº¥Åä¸® ¾ÆÀÌÅÛ Á¶È¸
        public const byte WShopInquiryInventory = 0x9B; //ÀÎº¥Åä¸® Á¶È¸	
        public const byte WShopPickUp = 0x9C; //ÀÎº¥Åä¸® ¼ö·É
        public const byte WShopCheckPickUp = 0x9D; //ÀÎº¥Åä¸® ¼ö·É °Ç È®ÀÎ
        public const byte WShopCancelPickUp = 0x9E; //ÀÎº¥Åä¸®¼ö·É°ÇÃë¼Ò
        public const byte WShopConfirmPickUp = 0x9F; //ÀÎº¥Åä¸®¼ö·É°Ç½ÂÀÎ
        //Jewel
        public const byte ChargeJewel = 0xB0; //Áê¾óÃæÀü¿äÃ»
        public const byte CheckJewelCharge = 0xB1; //Áê¾óÃæÀü°ÇÈ®ÀÎ
        public const byte CancelJewelCharge = 0xB2; //Áê¾óÃæÀü°ÇÃë¼Ò
        public const byte ConfirmJewelCharge = 0xB3; //Áê¾óÃæÀü°Ç½ÂÀÎ
        public const byte PurchaseJewelItem = 0xB4; //Áê¾ó¼ÒÁø
        public const byte CheckPurchaseJewel = 0xB5; //Áê¾ó¼ÒÁø°ÇÈ®ÀÎ
        public const byte CancelJewelPurchase = 0xB6; //Áê¾ó¼ÒÁø°ÇÃë¼Ò
        public const byte ConfirmJewelPurchase = 0xB7; //Áê¾ó¼ÒÁø°Ç½ÂÀÎ
        public const byte TradeJewel = 0xB8; //Áê¾ó°æ¸ÅÀå°Å·¡
        public const byte CheckTradeJewel = 0xB9; //Áê¾ó°æ¸ÅÀå°Å·¡°ÇÈ®ÀÎ
        public const byte CancelJewelTrade = 0xBA; //Áê¾ó°æ¸ÅÀå°Å·¡°ÇÃë¼Ò
        public const byte ConfirmJewelTrade = 0xBB; //Áê¾ó °æ¸ÅÀå °Å·¡°Ç ½ÂÀÎ
        public const byte PickUpTradeJewel = 0xBC; // Áê¾ó °æ¸ÅÀå ÆÇ¸Å±Ý¾× ¼ö·É(ÆÇ¸ÅÀÚ)
        public const byte CheckTradeJewelPickUp = 0xBD; // °æ¸ÅÀå ÆÇ¸Å±Ý¾× ¼ö·É°Ç È®ÀÎ
        public const byte CancelTradeJewelPickUp = 0xBE; // °æ¸ÅÀå ÆÇ¸Å±Ý¾× ¼ö·É°Ç Ãë¼Ò
        public const byte ConfirmTradeJewelPickUp = 0xBF; // °æ¸ÅÀå ÆÇ¸Å±Ý¾× ¼ö·É°Ç ½ÂÀÎ
        // Advanced Billing - Cash Inventory
        public const byte InquiryCashInventory = 0x69; //Ä³½ÃÀÎº¥Åä¸®Á¶È¸
        public const byte InquiryCashInventoryByBindAttribute = 0x81; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ Ä³½ÃÀÎº¥Åä¸®Á¶È¸
        public const byte PickupCashInventoryItem = 0x6B; //Ä³½ÃÀÎº¥Åä¸®¼ö·É
        public const byte PickupCashInventoryItemByBindAttribute = 0x82; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ Ä³½ÃÀÎº¥Åä¸®¼ö·É
        public const byte CancelCashInventoryItem = 0x6C; //Ä³½ÃÀÎº¥Åä¸®º¹±¸
        public const byte CancelCashInventoryItemByBindAttribute = 0x83; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ Ä³½ÃÀÎº¥Åä¸®º¹±¸
        public const byte PickupCashSubInventoryItem = 0x6D; //°³º°Ä³½ÃÀÎº¥Åä¸®¼ö·É
        public const byte PickupCashSubInventoryItemByBindAttribute = 0x84; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ °³º°Ä³½ÃÀÎº¥Åä¸®¼ö·É
        public const byte CancelCashSubInventoryItem = 0x6E; //°³º°Ä³½ÃÀÎº¥Åä¸®Ãë¼Ò
        public const byte CancelCashSubInventoryItemByBindAttribute = 0x85; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ °³º°Ä³½ÃÀÎº¥Åä¸®Ãë¼Ò
        public const byte InquiryPickupStandbyCashPurchaseNo = 0x86;   //°ÔÀÓ¿¡¼­ ¼ö·É°¡´ÉÇÑ ¾ÆÀÌÅÛ ±¸¸Å¹øÈ£ ¸®½ºÆ® Á¶È¸
        public const byte PickupStandbyCashInventory = 0x87;   //±¸¸Å ¹øÈ£¿¡ ÇØ´çÇÏ´Â ¾ÆÀÌÅÛÀ» °ÔÀÓ ³» ¼ö·É ´ë±â»óÅÂ·Î ÀüÈ¯
        public const byte CompletePickupStandbyCashInventory = 0x8C;   //±¸¸Å ¹øÈ£¿¡ ÇØ´çÇÏ´Â ¾ÆÀÌÅÛÀ» °ÔÀÓ ³» ¼ö·É ¿Ï·á»óÅÂ·Î ÀüÈ¯
        public const byte CancelPickupStandbyCashInventory = 0x8D; //±¸¸Å ¹øÈ£¿¡ ÇØ´çÇÏ´Â ¾ÆÀÌÅÛÀ» °ÔÀÓ ³» ¼ö·É ´ë±â»óÅÂ¿¡¼­ Ãë¼Ò
        public const byte UseStorage = 0x8E; //IBS º¸°üÇÔ »óÇ° »ç¿ë
        public const byte RollbackUseStorage = 0x8F; //IBS º¸°üÇÔ »ç¿ë ·Ñ¹é
        // Advanced Billing - Inquiry OData
        public const byte InquiryServiceMetadata = 0x74; //¼­ºñ½º¸ÞÅ¸µ¥ÀÌÅ¸Á¶È¸
        public const byte InquiryODataService = 0x75; //OData¼­ºñ½ºÁ¶È¸
        // Advanced Billing - Cart & Wish Items
        public const byte InquiryCartItems = 0xA1; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛÁ¶È¸
        public const byte RegisterCartItems = 0xA2; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛµî·Ï
        public const byte ModifyCartItemsQuantity = 0xA3; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛ¼ö·®º¯°æ
        public const byte ModifyCartItemsAttribute = 0xA4; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛ¼Ó¼ºº¯°æ
        public const byte RemoveCartItems = 0xA5; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛ»èÁ¦
        public const byte RemoveCart = 0xA6; //¼îÇÎÄ«Æ®ÀüÃ¼¾ÆÀÌÅÛ»èÁ¦
        public const byte CartItemsTransferWishItems = 0xA7; //À§½Ã¾ÆÀÌÅÛÀ¸·Î¼îÇÎÄ«Æ®¾ÆÀÌÅÛÀÌµ¿
        public const byte InquiryWishItems = 0xAB; //À§½Ã¾ÆÀÌÅÛÁ¶È¸
        public const byte RegisterWishItems = 0xAC; //À§½Ã¾ÆÀÌÅÛµî·Ï
        public const byte RemoveWishItems = 0xAD; //À§½Ã¾ÆÀÌÅÛ»èÁ¦
        public const byte RemoveWish = 0xAE; //À§½Ã¾ÆÀÌÅÛÀüÃ¼»èÁ¦
        public const byte WishItemsTransferCartItems = 0xAF; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛÀ¸·ÎÀ§½Ã¾ÆÀÌÅÛÀÌµ¿
        // Advanced Billing - Order
        public const byte ShopDirectPurchaseItem = 0xC9; //¹Ù·Î±¸¸Å
        public const byte ShopDirectPurchaseItemByBindAttribute = 0x88; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ ¹Ù·Î±¸¸Å
        public const byte ShopPurchaseCartItems = 0xCA; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛ±¸¸Å
        public const byte ShopDirectGiftItem = 0xCB; //¹Ù·Î¼±¹°ÇÏ±â
        public const byte ShopDirectGiftItemByBindAttribute = 0x89; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ ¹Ù·Î¼±¹°ÇÏ±â
        public const byte ShopGiftCartItems = 0xCC; //¼îÇÎÄ«Æ®¾ÆÀÌÅÛ¼±¹°
        // Advanced Billing - Coupon									
        public const byte CheckCoupon = 0xDD; //ÄíÆùÁ¶È¸
        public const byte CheckCouponByBindAttribute = 0x8A; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ ÄíÆùÁ¶È¸
        public const byte UseCoupon = 0xDE; //ÄíÆù»ç¿ë
        public const byte UseCouponByBindAttribute = 0x8B; //¾ÆÀÌÅÛ ±Í¼Ó ÇüÅÂ¿¡ µû¸¥ ÄíÆù»ç¿ë
    }
}
