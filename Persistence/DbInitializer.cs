namespace Persistence;
using Domain; 

public class DbInitializer
{
    public static async Task SeedData(AppDbContext context)
    {
        // 1. 检查并填充位置信息
        if (!context.Location.Any())
        {
            var locations = new List<Location>
            {
                new() { LocationName = "总部", LocationType = "总部", Address = "公司总部地址" },
                new() { LocationName = "城西第一药店", LocationType = "药店", Address = "城西街道123号" },
                new() { LocationName = "城东第二药店", LocationType = "药店", Address = "城东路456号" },
                new() { LocationName = "城南第三药店", LocationType = "药店", Address = "城南大道789号" },
                new() { LocationName = "官方授权维修中心", LocationType = "修理厂", Address = "工业园区A栋" }
            };
            
            await context.Location.AddRangeAsync(locations);
            await context.SaveChangesAsync(); // 先保存位置信息，获取自动生成的ID
        }

        // 2. 检查并填充打印机信息
        if (!context.Printers.Any())
        {
            // 获取已保存的位置信息
            var locations = context.Location.ToList();
            var headquartersId = locations.First(l => l.LocationName == "总部").Id;
            var store1Id = locations.First(l => l.LocationName == "城西第一药店").Id;
            var store2Id = locations.First(l => l.LocationName == "城东第二药店").Id;
            var store3Id = locations.First(l => l.LocationName == "城南第三药店").Id;

            var printers = new List<Printer>
            {
                new()
                {
                    Brand = "HP",
                    ModelType = "LaserJet Pro M404dn",
                    TonerModel = "HP 58A",
                    SerialNumber = "HP001234567890",
                    Specification = "A4激光打印机",
                    LocationID = headquartersId,
                    Status = "正常",
                    PurchaseDate = DateTime.Now.AddMonths(-6),
                    WarrantyExpiryDate = DateTime.Now.AddYears(2),
                    Notes = "总部主要打印设备"
                },
                new()
                {
                    Brand = "Epson",
                    ModelType = "LQ-630K",
                    TonerModel = "Epson S015290",
                    SerialNumber = "EP001234567890",
                    Specification = "A5针式打印机",
                    LocationID = store1Id,
                    Status = "正常",
                    PurchaseDate = DateTime.Now.AddMonths(-3),
                    WarrantyExpiryDate = DateTime.Now.AddYears(1),
                    Notes = "城西药店收据打印机"
                },
                new()
                {
                    Brand = "Canon",
                    ModelType = "PIXMA G3800",
                    TonerModel = "Canon GI-490",
                    SerialNumber = "CN001234567890",
                    Specification = "A4彩色喷墨一体机",
                    LocationID = store2Id,
                    Status = "维修中",
                    PurchaseDate = DateTime.Now.AddMonths(-4),
                    WarrantyExpiryDate = DateTime.Now.AddYears(1),
                    Notes = "城东药店多功能打印设备"
                },
                new()
                {
                    Brand = "Brother",
                    ModelType = "MFC-L2715DW",
                    TonerModel = "Brother TN-2325",
                    SerialNumber = "BR001234567890",
                    Specification = "A4激光一体机",
                    LocationID = store3Id,
                    Status = "正常",
                    PurchaseDate = DateTime.Now.AddMonths(-2),
                    WarrantyExpiryDate = DateTime.Now.AddYears(3),
                    Notes = "城南药店激光一体机"
                }
            };
            
            await context.Printers.AddRangeAsync(printers);
            await context.SaveChangesAsync(); // 保存打印机信息，获取自动生成的ID
        }

        // 3. 检查并填充转移日志
        if (!context.TransferLog.Any())
        {
            var printers = context.Printers.ToList();
            var locations = context.Location.ToList();
            var headquartersId = locations.First(l => l.LocationName == "总部").Id;
            
            var transferLogs = new List<TransferLog>();
            
            // 为每台打印机创建初始入库记录
            foreach(var printer in printers)
            {
                transferLogs.Add(new TransferLog
                {
                    PrinterID = printer.Id,
                    FromLocationID = headquartersId, // 假设都是从总部分发出去的
                    ToLocationID = printer.LocationID,
                    TransferDate = printer.PurchaseDate ?? DateTime.Now.AddDays(-30),
                    TransferredBy = "系统管理员",
                    TransferReason = "库存补充",
                    Notes = $"初始分配 - {printer.Brand} {printer.ModelType} 分配到 {printer.Location?.LocationName}"
                });
            }

            // 添加一些示例转移记录
            var hpPrinter = printers.FirstOrDefault(p => p.Brand == "HP");
            var store2Id = locations.First(l => l.LocationName == "城东第二药店").Id;
            
            if (hpPrinter != null)
            {
                transferLogs.Add(new TransferLog
                {
                    PrinterID = hpPrinter.Id,
                    FromLocationID = headquartersId,
                    ToLocationID = store2Id,
                    TransferDate = DateTime.Now.AddDays(-15),
                    TransferredBy = "张经理",
                    TransferReason = "门店调配",
                    Notes = "临时调配到城东药店支援"
                });
            }
            
            await context.TransferLog.AddRangeAsync(transferLogs);
        }

        // 最后保存所有变更
        await context.SaveChangesAsync();
    }
}