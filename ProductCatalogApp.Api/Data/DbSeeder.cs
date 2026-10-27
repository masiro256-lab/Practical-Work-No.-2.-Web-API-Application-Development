using ProductCatalogApp.Api.Models;

namespace ProductCatalogApp.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.ProductionSites.Any())
            {
                return;
            }

            context.ProductionSites.AddRange(
                new ProductionSite
                {
                    Name = "Доменный цех",
                    Description = "Выплавка чугуна из железной руды",
                    EquipmentItems =
                    {
                        new Equipment { Name = "Доменная печь", Model = "ДП-5", InventoryNumber = "INV-0001" },
                        new Equipment { Name = "Воздухонагреватель", Model = "ВН-3", InventoryNumber = "INV-0002" }
                    }
                },
                new ProductionSite
                {
                    Name = "Сталеплавильный цех",
                    Description = "Переработка чугуна в сталь",
                    EquipmentItems =
                    {
                        new Equipment { Name = "Кислородный конвертер", Model = "КК-250", InventoryNumber = "INV-0003" },
                        new Equipment { Name = "Машина непрерывного литья заготовок", Model = "МНЛЗ-4", InventoryNumber = "INV-0004" },
                        new Equipment { Name = "Мостовой кран", Model = "МК-100", InventoryNumber = "INV-0005" }
                    }
                },
                new ProductionSite
                {
                    Name = "Прокатный цех",
                    Description = "Прокатка слябов в листовой металл"
                });

            context.SaveChanges();
        }
    }
}
