using Mailoo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mailo.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayPalService _payPalService;

        public PaymentController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public ActionResult PaymentWithPayPal()
        {
            var redirectUrl = "https://your-site.com/Payment/PaymentSuccess";
            var payment = _payPalService.CreatePayment(redirectUrl);

            var approvalUrl = payment.links.FirstOrDefault(l => l.rel.Equals("approval_url", System.StringComparison.OrdinalIgnoreCase)).href;

            return Redirect(approvalUrl);
        }

        public ActionResult PaymentSuccess()
        {

            return View();
        }
    }
}