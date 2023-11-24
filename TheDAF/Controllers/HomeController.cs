using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using TheDAF.Models;

namespace TheDAF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Part 1
        //Connection string 
        public string connectionString = "Server=tcp:admin01.database.windows.net,1433;Initial Catalog=data-infor;Persist Security Info=False;User ID=admin59;Password=@sethibe59;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //Capturing users(admin) details, register and login details
        //Register capturing 
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO UserDetails (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Menu", "Home");
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM UserDetails WHERE Username = @Username AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Authentication successful
                            return RedirectToAction("Menu", "Home"); 
                        }
                        else
                        {
                            // Authentication failed
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(user);
                        }
                    }
                }
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //Admin capuates deteails about donations and the details being sent and saved to the Azure Sql database
        //Monetary donation table
        [HttpPost]
        public IActionResult MonetaryDonation(MonetaryDonationModel monetaryDonation)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO MoneyDonations (Date, Amount, IsAnonymous) VALUES (@Date, @Amount, @IsAnonymous)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", monetaryDonation.Date);
                        cmd.Parameters.AddWithValue("@Amount", monetaryDonation.Amount);
                        cmd.Parameters.AddWithValue("@IsAnonymous", monetaryDonation.IsAnonymous);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Menu", "Home"); // Redirect to the home page after successful donation
            }

            return View(monetaryDonation);
        }
        [HttpGet]
        public IActionResult MonetaryDonation()
        {
            return View();
        }

        //View Monetary Goods
        public IActionResult ViewMonetaryDonations()
        {
            var monetaryDonations = GetMonetaryDonationsFromDatabase();
            return View(monetaryDonations);
        }
        private List<MonetaryDonationModel> GetMonetaryDonationsFromDatabase()
        {
            List<MonetaryDonationModel> monetaryDonations = new List<MonetaryDonationModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM MoneyDonations";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monetaryDonations.Add(new MonetaryDonationModel
                            {
                                
                                Date = (DateTime)reader["Date"],
                                Amount = (decimal)reader["Amount"],
                                IsAnonymous = (bool)reader["IsAnonymous"]
                            });
                        }
                    }
                }
            }

            return monetaryDonations;
        }


        //Goods donations table
        [HttpPost]
        public IActionResult GoodsDonation(GoodsDonationModel goodsDonation)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO GoodsDonations (Date, NoOfItems, Category, Description,IsAnonymous) VALUES (@Date, @NoOfItems, @Category, @Description,@IsAnonymous)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", goodsDonation.Date);
                        cmd.Parameters.AddWithValue("@NoOfItems", goodsDonation.NoOfItems);
                        cmd.Parameters.AddWithValue("@Category", goodsDonation.Category);
                        cmd.Parameters.AddWithValue("@Description", goodsDonation.Description);
                        cmd.Parameters.AddWithValue("@IsAnonymous", goodsDonation.IsAnonymous);

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Menu", "Home"); // Redirect to the home page after successful donation
            }

            return View(goodsDonation);
        }
        [HttpGet]
        public IActionResult GoodsDonation()
        {
            return View();
        }

        //View Goods
        public IActionResult ViewGoodsDonations()
        {
            var goodsDonations = GetGoodsDonationsFromDatabase();
            return View(goodsDonations);
        }

        private List<GoodsDonationModel> GetGoodsDonationsFromDatabase()
        {
            List<GoodsDonationModel> goodsDonations = new List<GoodsDonationModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM GoodsDonations";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            goodsDonations.Add(new GoodsDonationModel
                            {
                               
                                Date = (DateTime)reader["Date"],
                                NoOfItems = (int)reader["NoOfItems"],
                                Category = reader["Category"].ToString(),
                                Description = reader["Description"].ToString(),
                                IsAnonymous = (bool)reader["IsAnonymous"]
                            });
                        }
                    }
                }
            }

            return goodsDonations;
        }

        //Admin being able to add a category
        //Category table
        [HttpPost]
        public IActionResult Category(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Menu", "Home");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Category()
        {
            return View();
        }

        //Admin caturing a disaster 
        //disaster table
        [HttpPost]
        public async Task<IActionResult> Disaster(DisasterModel disaster)
        {
            if (ModelState.IsValid)
            {
                // Save disaster to the database
                InsertDisasterIntoDatabase(disaster);

                return RedirectToAction("Menu");
            }

            return View(disaster);
        }

        private void InsertDisasterIntoDatabase(DisasterModel disaster)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Disasters (StartDate, EndDate, Location, Description, AidType, Active) " +
                               "VALUES (@StartDate, @EndDate, @Location, @Description, @AidType, @Active)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", disaster.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", disaster.EndDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Location", disaster.Location);
                    cmd.Parameters.AddWithValue("@Description", disaster.Description);
                    cmd.Parameters.AddWithValue("@AidType", disaster.AidType);
                    cmd.Parameters.AddWithValue("@Active", disaster.Active);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        [HttpGet]
        public IActionResult Disaster()
        {
            return View();
        }

        //Viewing the disasters
        public IActionResult DisasterViews()
        {
            // Retrieve and display existing disasters
            var disasters = GetDisastersFromDatabase();
            return View(disasters);
        }
        private List<DisasterModel> GetDisastersFromDatabase()
        {
            List<DisasterModel> disasters = new List<DisasterModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Disasters";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            disasters.Add(new DisasterModel
                            {
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = reader["EndDate"] is DBNull ? (DateTime?)null : (DateTime)reader["EndDate"],
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                AidType = reader["AidType"].ToString(),
                                Active = (bool)reader["Active"]
                            });
                        }
                    }
                }
            }

            return disasters;
        }
        //End of Part 1

        //Part 2
        //Admin capturing allocation of Money and Goods donation to a disaster
        //Money donation allocate table
        [HttpPost]
        public IActionResult MonetaryDonationAllocation(MoneyDonationAllocateModel allocation)
        {
            if (ModelState.IsValid)
            {
                if (IsDisasterActive(allocation.DisasterID))
                {
                    InsertMoneyDonationAllocationIntoDatabase(allocation);
                    return RedirectToAction("Menu");
                }
                else
                {
                    ModelState.AddModelError("DisastersID", "Selected disaster is not active.");
                }
            }

            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase1();
            ViewBag.ActiveDisasters = activeDisasters;

            return View(allocation);
        }

        [HttpGet]
        public IActionResult MonetaryDonationAllocation()
        {
            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase();
            ViewBag.ActiveDisasters = activeDisasters;

            return View();
        }
        private bool IsDisasterActive(int disasterID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StartDate, EndDate FROM Disasters WHERE DisastersID = @DisastersID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", disasterID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime startDate = (DateTime)reader["StartDate"];
                            DateTime? endDate = reader["EndDate"] != DBNull.Value ? (DateTime?)reader["EndDate"] : null;

                            // Check if the current date is within the start and end date range
                            DateTime currentDate = DateTime.Now;
                            return currentDate >= startDate && (!endDate.HasValue || currentDate <= endDate.Value);
                        }
                    }
                }
            }

            // Default to false if disaster is not found or there is an issue with the database
            return false;
        }

        private void InsertMoneyDonationAllocationIntoDatabase(MoneyDonationAllocateModel allocation)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO MoneyDonationAllocation (DisastersID, Description, Amount) " +
                               "VALUES (@DisastersID, @Description, @Amount)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", allocation.DisasterID);
                    cmd.Parameters.AddWithValue("@Description", allocation.Description);
                    cmd.Parameters.AddWithValue("@Amount", allocation.Amount);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        //View Money Allocations
        public IActionResult MoneyDonationAllocationView()
        {
            var allocations = GetMoneyDonationAllocationsFromDatabase();
            return View(allocations);
        }

        private List<MoneyDonationAllocateModel> GetMoneyDonationAllocationsFromDatabase()
        {
            var allocations = new List<MoneyDonationAllocateModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM MoneyDonationAllocation";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allocations.Add(new MoneyDonationAllocateModel
                            {
                                DisasterID = (int)reader["DisastersID"],
                                Description = reader["Description"].ToString(),
                                Amount = (decimal)reader["Amount"]
                            });
                        }
                    }
                }
            }

            return allocations;
        }

        private List<DisasterModel> GetDisastersFromDatabase1()
        {
            List<DisasterModel> disasters = new List<DisasterModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Disasters";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            disasters.Add(new DisasterModel
                            {
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = reader["EndDate"] is DBNull ? (DateTime?)null : (DateTime)reader["EndDate"],
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                AidType = reader["AidType"].ToString(),
                                Active = (bool)reader["Active"]
                            });
                        }
                    }
                }
            }

            return disasters;
        }

        //GoodsAllocations
        [HttpPost]
        public IActionResult GoodsDonationAllocation(GoodsDonationAllocationModel allocation)
        {
            if (ModelState.IsValid)
            {
                if (IsDisasterActive1(allocation.DisasterID))
                {
                    InsertGoodsDonationAllocationIntoDatabase(allocation);
                    return RedirectToAction("Menu");
                }
                else
                {
                    ModelState.AddModelError("DisastersID", "Selected disaster is not active.");
                }
            }

            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase2();
            ViewBag.ActiveDisasters = activeDisasters;

            return View(allocation);
        }
        [HttpGet]
        public IActionResult GoodsDonationAllocation()
        {
            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase();
            ViewBag.ActiveDisasters = activeDisasters;

            return View();
        }
        private bool IsDisasterActive1(int disasterID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StartDate, EndDate FROM Disasters WHERE DisastersID = @DisasterID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisasterID", disasterID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime startDate = (DateTime)reader["StartDate"];
                            DateTime? endDate = reader["EndDate"] != DBNull.Value ? (DateTime?)reader["EndDate"] : null;

                            // Check if the current date is within the start and end date range
                            DateTime currentDate = DateTime.Now;
                            return currentDate >= startDate && (!endDate.HasValue || currentDate <= endDate.Value);
                        }
                    }
                }
            }

            // Default to false if disaster is not found or there is an issue with the database
            return false;
        }

        private void InsertGoodsDonationAllocationIntoDatabase(GoodsDonationAllocationModel allocation)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO GoodsDonationAllocation (DisastersID, Description, Description_Goods, NoOfItems) " +
                               "VALUES (@DisastersID, @Description, @Description_Goods, @NoOfItems)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", allocation.DisasterID);
                    cmd.Parameters.AddWithValue("@Description", allocation.Description);
                    cmd.Parameters.AddWithValue("@Description_Goods", allocation.Description_Goods);
                    cmd.Parameters.AddWithValue("@NoOfItems", allocation.NoOfItems);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private List<DisasterModel> GetDisastersFromDatabase2()
        {
            List<DisasterModel> disasters = new List<DisasterModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Disasters";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            disasters.Add(new DisasterModel
                            {
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = reader["EndDate"] is DBNull ? (DateTime?)null : (DateTime)reader["EndDate"],
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                AidType = reader["AidType"].ToString(),
                                Active = (bool)reader["Active"]
                            });
                        }
                    }
                }
            }

            return disasters;
        }

        //View Goods donations allocated to a disaster
        public IActionResult GoodsDonationAllocationsView()
        {
            var goodsAllocations = GetGoodsDonationAllocationsFromDatabase();
            return View(goodsAllocations);
        }

        private List<GoodsDonationAllocationModel> GetGoodsDonationAllocationsFromDatabase()
        {
            var allocations = new List<GoodsDonationAllocationModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM GoodsDonationAllocation";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allocations.Add(new GoodsDonationAllocationModel
                            {
                                DisasterID = (int)reader["DisastersID"],
                                Description = reader["Description"].ToString(),
                                Description_Goods = reader["Description_Goods"].ToString(),
                                NoOfItems = reader["NoOfItems"].ToString()
                            });
                        }
                    }
                }
            }

            return allocations;
        }

        //Capturing the purchases made by organization, by the admin
        [HttpPost]
        public IActionResult Purchase(PurchaseMadeModel purchase)
        {
            if (ModelState.IsValid)
            {
                if (IsDisasterActive(purchase.DisasterID))
                {
                    // Check if there is enough money in the disaster's fund
                    decimal availableMoney = GetAvailableMoneyForDisaster(purchase.DisasterID);

                    if (availableMoney >= purchase.TotalCost)
                    {
                        InsertPurchaseMadeIntoDatabase(purchase);
                        return RedirectToAction("Menu");
                    }
                    else
                    {
                        ModelState.AddModelError("TotalCost", "Insufficient funds in the disaster's fund.");
                    }
                }
                else
                {
                    ModelState.AddModelError("DisastersID", "Selected disaster is not active.");
                }
            }

            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase();
            ViewBag.ActiveDisasters = activeDisasters;

            return View(purchase);
        }


        [HttpGet]
        public IActionResult Purchase()
        {
            // Fetch active disasters to display in a dropdown for selection
            var activeDisasters = GetDisastersFromDatabase();
            ViewBag.ActiveDisasters = activeDisasters;

            return View();
        }

        private bool IsDisasterActive2(int disasterID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StartDate, EndDate FROM Disasters WHERE DisastersID = @DisastersID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", disasterID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime startDate = (DateTime)reader["StartDate"];
                            DateTime? endDate = reader["EndDate"] != DBNull.Value ? (DateTime?)reader["EndDate"] : null;

                            // Check if the current date is within the start and end date range
                            DateTime currentDate = DateTime.Now;
                            return currentDate >= startDate && (!endDate.HasValue || currentDate <= endDate.Value);
                        }
                    }
                }
            }

            // Default to false if disaster is not found or there is an issue with the database
            return false;
        }

        private decimal GetAvailableMoneyForDisaster(int disasterID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT SUM(Amount) AS TotalMoney FROM MoneyDonationAllocation WHERE DisastersID = @DisastersID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", disasterID);

                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                }
            }

            // Default to 0 if there is an issue with the database
            return 0;
        }

        private void InsertPurchaseMadeIntoDatabase(PurchaseMadeModel purchase)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO PurchaseMade (DisastersID, Description, PurchasedItems, TotalCost) " +
                               "VALUES (@DisastersID, @Description, @PurchasedItems, @TotalCost)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DisastersID", purchase.DisasterID);
                    cmd.Parameters.AddWithValue("@Description", purchase.Description);
                    cmd.Parameters.AddWithValue("@PurchasedItems", purchase.PurchasedItems);
                    cmd.Parameters.AddWithValue("@TotalCost", purchase.TotalCost);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private List<DisasterModel> GetDisastersFromDatabase3()
        {
            List<DisasterModel> disasters = new List<DisasterModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Disasters";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            disasters.Add(new DisasterModel
                            {
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = reader["EndDate"] is DBNull ? (DateTime?)null : (DateTime)reader["EndDate"],
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                AidType = reader["AidType"].ToString(),
                                Active = (bool)reader["Active"]
                            });
                        }
                    }
                }
            }

            return disasters;
        }

        //View the purchases made
        public IActionResult PurchasesView()
        {
            var purchases = GetPurchasesFromDatabase();
            return View(purchases);
        }

        private List<PurchaseMadeModel> GetPurchasesFromDatabase()
        {
            var purchases = new List<PurchaseMadeModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM PurchaseMade";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchases.Add(new PurchaseMadeModel
                            {
                                PurchaseMadeID = (int)reader["PurchaseMadeID"],
                                DisasterID = (int)reader["DisastersID"],
                                Description = reader["Description"].ToString(),
                                PurchasedItems = reader["PurchasedItems"].ToString(),
                                TotalCost = (decimal)reader["TotalCost"]
                            });
                        }
                    }
                }
            }

            return purchases;
        }

        //View remaining amount
        public IActionResult RemainingAmount()
        {
            var remainingAmounts = GetRemainingAmountsFromDatabase();
            return View(remainingAmounts);
        }

        private List<RemainingAmountModel> GetRemainingAmountsFromDatabase()
        {
            var remainingAmounts = new List<RemainingAmountModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT m.DisastersID, SUM(m.Amount) - COALESCE(SUM(p.TotalCost), 0) AS RemainingAmount " +
                               "FROM MoneyDonationAllocation m " +
                               "LEFT JOIN PurchaseMade p ON m.DisastersID = p.DisastersID " +
                               "GROUP BY m.DisastersID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            remainingAmounts.Add(new RemainingAmountModel
                            {
                                DisasterID = (int)reader["DisastersID"],
                                RemainingAmount = (decimal)reader["RemainingAmount"]
                            });
                        }
                    }
                }
            }

            return remainingAmounts;
        }

        //Menu for user to select page to work on
        public IActionResult Menu()
        {
            return View();
        }

        //Part 2 end
        //Part 3
        //a user being about to view information as a common website browser
        public IActionResult PublicInfo()
        {
            var model = GetPublicInfoFromDatabase();
            return View(model);
        }

        private PublicInfoModel GetPublicInfoFromDatabase()
        {
            var publicInfo = new PublicInfoModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve total monetary donations received
                string monetaryDonationQuery = "SELECT COALESCE(SUM(Amount), 0) AS TotalMonetaryDonations FROM MoneyDonations";
                using (SqlCommand monetaryDonationCmd = new SqlCommand(monetaryDonationQuery, connection))
                {
                    publicInfo.TotalMonetaryDonations = (decimal)monetaryDonationCmd.ExecuteScalar();
                }

                // Retrieve total number of goods received
                string goodsDonationQuery = "SELECT COALESCE(SUM(NoOfItems), 0) AS TotalGoodsReceived FROM GoodsDonations";
                using (SqlCommand goodsDonationCmd = new SqlCommand(goodsDonationQuery, connection))
                {
                    publicInfo.TotalGoodsReceived = (int)goodsDonationCmd.ExecuteScalar();
                }

                // Retrieve currently active disasters with allocated money and goods
                string activeDisastersQuery = @"
                SELECT 
                    d.DisastersID, d.StartDate, d.EndDate, d.Location, d.Description, d.AidType, 
                    COALESCE(SUM(m.Amount), 0) AS AllocatedMoney, 
                    COALESCE(SUM(g.NoOfItems), 0) AS AllocatedGoods 
                FROM 
                    Disasters d 
                    LEFT JOIN MoneyDonationAllocation m ON d.DisastersID = m.DisastersID 
                    LEFT JOIN GoodsDonationAllocation g ON d.DisastersID = g.DisastersID 
                WHERE 
                    d.Active = 1 
                    AND (d.EndDate IS NULL OR d.EndDate >= GETDATE())
                GROUP BY 
                    d.DisastersID, d.StartDate, d.EndDate, d.Location, d.Description, d.AidType";

                using (SqlCommand activeDisastersCmd = new SqlCommand(activeDisastersQuery, connection))
                {
                    using (SqlDataReader reader = activeDisastersCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var disasterInfo = new DisasterInfoModel
                            {
                                DisasterID = (int)reader["DisastersID"],
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = reader["EndDate"] != DBNull.Value ? (DateTime)reader["EndDate"] : (DateTime?)null,
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                AidType = reader["AidType"].ToString(),
                                AllocatedMoney = (decimal)reader["AllocatedMoney"],
                                AllocatedGoods = (int)reader["AllocatedGoods"]
                            };
                            publicInfo.ActiveDisasters.Add(disasterInfo);
                        }
                    }
                }
            }

            return publicInfo;
        }
    }

}