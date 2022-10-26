using System;
namespace odeba.Models
{
    public class RecycleItemModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public MachineModel ProcessedMachine { get; set; }
        public double Weight { get; set; }
        public string Manufacturer { get; set; }
        public string QRCodeId { get; set; }
    }
}

