using System.ComponentModel;

namespace Entities.Enums
{
    public enum BodyTypeEnum
    {
        [Description("")]
        Unknown = 0,

        [Description("Sedan")]
        Sedan = 1,

        [Description("Hetçbek, 5 qapı")]
        Hatchback5Door = 2,

        [Description("Kupe")]
        Coupe = 3,

        [Description("Universal, 5 qapı")]
        StationWagon5Door = 4,

        [Description("Minivan")]
        Minivan = 5,

        [Description("Pikap, ikiqat kabin")]
        PickupDoubleCabin = 6,

        [Description("Mikroavtobus")]
        Minibus = 7,

        [Description("Rodster")]
        Roadster = 8,

        [Description("Avtobus")]
        Bus = 9,

        [Description("Yük maşını")]
        Truck = 13,

        [Description("Furqon")]
        VanFurqon = 14,

        [Description("Dartqı")]
        Tractor = 16,

        [Description("Qolfkar")]
        GolfCart = 22,

        [Description("Offroader / SUV, 5 qapı")]
        Suv5Door = 21,

        [Description("Motosiklet")]
        Motorcycle = 20,

        [Description("Kabriolet")]
        Convertible = 11,

        [Description("Karvan")]
        Caravan = 26,

        [Description("Kvadrosikl")]
        QuadBike = 25,

        [Description("Liftbek")]
        Liftback = 28,

        [Description("Kompakt-Van")]
        CompactVan = 65,

        [Description("Limuzin")]
        Limousine = 66,

        [Description("Mikrovan")]
        Microvan = 67,

        [Description("Moped")]
        Moped = 27,

        [Description("Offroader / SUV, 3 qapı")]
        Suv3Door = 68,

        [Description("Offroader / SUV, açıq")]
        SuvOpen = 69,

        [Description("Pikap, bir yarım kabin")]
        PickupOneAndHalfCabin = 70,

        [Description("Pikap, tək kabin")]
        PickupSingleCabin = 71,

        [Description("Spidster")]
        Speedster = 73,

        [Description("SUV Kupe")]
        SuvCoupe = 72,

        [Description("Tarqa")]
        Targa = 74,

        [Description("Universal, 3 qapı")]
        StationWagon3Door = 75,

        [Description("Hetçbek, 3 qapı")]
        Hatchback3Door = 63,

        [Description("Hetçbek, 4 qapı")]
        Hatchback4Door = 64,

        [Description("Fastbek")]
        Fastback = 61,

        [Description("Fayton")]
        Phaeton = 62,

        [Description("Van")]
        Van = 19,

        [Description("Offroader / SUV, 3 qapı")]
        Suv3DoorAlt = 68,

        [Description("Qolfkar")]
        GolfCartAlt = 22
    }
}
