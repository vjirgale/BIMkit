using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DbmsApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ObjectTypes
    {
        Root = 1,
        
        Virtual = 2,
            Building = 3,
            BuildingStorey = 4,
            Corner = 5,
            FurnishingCoM = 6,
            KitchenIsland = 7,
            KitchenPeninsula = 8,
            KitchenLandingArea = 9,
            KitchenWorkingTriangle = 10,
            OpenSpace = 11,
            Room = 12,
            RoomCentroid = 13,
            Site = 14,
            WalkPath = 15,
        Real = 100,
            BuildingElement = 101,
                Beam = 102,
                    Joist = 103,
                Chimney  = 107,
                Column = 108,
                    Stud = 109,
                Covering = 110,
                //Footing = 111,
                //Member = 200,
                Opening = 201,
                    Door = 202,
                    Window = 210,
                Pile = 230,
                Plate = 231,
                Railing = 232,
                Ramp = 233,
                RampFlight = 234,
                Roof = 235,
                ShadingDevice = 245,
                Slab = 250,
                    CounterTop = 251,
                    Floor = 252,
                    Wall = 260,
                        CurtainWall = 261,
                Stair = 280,
                StairFlight = 281,
            FurnishingElement = 500,
                Couch = 501,
                Chair = 510,
                Table = 520,
                Bed = 530,
                Plant = 540,
                Cabinet = 550,
                    WallCabinet = 551,
                    CornerCabinet = 552,
                    BaseCabinet = 555,
                Shelf = 570,
                Container = 580,
        DistributionElement = 1000,
            DistributionControlElement = 1001,
        //      Actuator
                Alarm = 1010,
        //      Controller
        //      FlowInstrument
        //      ProtectiveDeviceTrippingUnit
                Sensor = 1050,
        //      UnitaryControlElement
            DistributionFlowElement = 1200,
                DistributionChamberElement = 1201,
        //          EnergyConversionDevice
        //              AirToAirHeatRecovery
        //              Boiler
        //              Burner
        //              Chiller
        //              Coil
        //              Condenser
        //              CooledBeam
        //              CoolingTower
        //              ElectricGenerator
        //              ElectricMotor
        //              Engine
        //              EvaporativeCooler
        //              Evaporator
        //              HeatExchanger
        //              Humidifier
        //              MotorConnection
        //              SolarDevice
        //              Transformer
        //              TubeBundle
        //              UnitaryEquipment
        //          FlowController
        //              AirTerminalBox
        //              Damper
        //              ElectricDistributionBoard
        //              ElectricTimeControl
        //              FlowMeter
        //              ProtectiveDevice
        //              SwitchingDevice
        //              Valve
        //          FlowFitting
        //              CableCarrierFitting
        //              CableFitting
        //              DuctFitting
        //              JunctionBox
        //              PipeFitting
        //          FlowMovingDevice
        //              Compressor
        //              Fan
        //              Pump
        //          FlowSegment
        //              CableCarrierSegment
        //              CableSegment
        //              DuctSegment
        //              PipeSegment
                    FlowStorageDevice = 1300,
                        ElectricFlowStorageDevice = 1301,
                            Battery = 1302,
                        Tank = 1310,
                    FlowTerminal = 1400,
                        AirTerminal = 1401,
                            Fan = 1402,
                        AudioVisualAppliance = 1410,
                            Radio = 1411,
                            Television = 1412,
                            Speakers = 1413,
                        CommunicationsAppliance = 1450,
                            Telephone = 1451,
                        ElectricAppliance = 1500,
                            Range = 1501,
                            Stove = 1502,
                            Oven = 1503,
                            CookTop = 1504,
                            Refrigerator = 1505,
                            Microwave = 1506,
                            Dyer = 1507,
                        FireSuppressionTerminal = 1600,
                        LightFixture = 1610,
                            Lamp = 1611,
                        MedicalDevice = 1650,
                        Outlet = 1700,
                        SanitaryTerminal = 1720,
                            Sink = 1721,
                            Toilet = 1722,
                            WashingMashine = 1723,
                            Shower = 1724,
                            Bath = 1725,
                        SpaceHeater = 1800
        //              StackTerminal
        //              WasteTerminal
        //          FlowTreatmentDevice
        //              DuctSilencer
        //              Filter
        //              Interceptor
    }

    public class ObjectType
    {
        public readonly ObjectTypes ID;
        public readonly ObjectType Parent;
        private List<ObjectType> _Children = new List<ObjectType>();

        public ReadOnlyCollection<ObjectType> Children
        {
            get
            {
                return _Children.AsReadOnly();
            }
        }

        public ObjectType(ObjectTypes ID, ObjectType Parent)
        {
            this.ID = ID; 
            this.Parent = Parent;
            if (this.Parent != null)
            {
                this.Parent._Children.Add(this);
            }
        }
    }

    public static class ObjectTypeTree
    {
        public static readonly ObjectType Root = BuildTypeTree();
        public static readonly Dictionary<ObjectTypes, ObjectType> ObjectDict = traverse(Root);

        public static ObjectType GetNode(this ObjectTypes ot)
        {
            return ObjectDict[ot];
        }

        private static ObjectType BuildTypeTree()
        {
            ObjectType Root = new ObjectType(ObjectTypes.Root, null);
            ObjectType Virtual = new ObjectType(ObjectTypes.Virtual, Root);
            ObjectType Building = new ObjectType(ObjectTypes.Building, Virtual);
            ObjectType BuildingStorey = new ObjectType(ObjectTypes.BuildingStorey, Virtual);
            ObjectType Corner = new ObjectType(ObjectTypes.Corner, Virtual);
            ObjectType FurnishingCoM = new ObjectType(ObjectTypes.FurnishingCoM, Virtual);
            ObjectType KitchenIsland = new ObjectType(ObjectTypes.KitchenIsland, Virtual);
            ObjectType KitchenPeninsula = new ObjectType(ObjectTypes.KitchenPeninsula, Virtual);
            ObjectType KitchenLandingArea = new ObjectType(ObjectTypes.KitchenLandingArea, Virtual);
            ObjectType KitchenWorkingTriangle = new ObjectType(ObjectTypes.KitchenWorkingTriangle, Virtual);
            ObjectType OpenSpace = new ObjectType(ObjectTypes.OpenSpace, Virtual);
            ObjectType Room = new ObjectType(ObjectTypes.Room, Virtual);
            ObjectType RoomCentroid = new ObjectType(ObjectTypes.RoomCentroid, Virtual);
            ObjectType Site = new ObjectType(ObjectTypes.Site, Virtual);
            ObjectType WalkPath = new ObjectType(ObjectTypes.WalkPath, Virtual);

            ObjectType Real = new ObjectType(ObjectTypes.Real, Root);
            ObjectType BuildingElement = new ObjectType(ObjectTypes.BuildingElement, Real);
            ObjectType Beam = new ObjectType(ObjectTypes.Beam, BuildingElement);
            ObjectType Joist = new ObjectType(ObjectTypes.Joist, Beam);
            ObjectType Chimney = new ObjectType(ObjectTypes.Chimney, BuildingElement);
            ObjectType Column = new ObjectType(ObjectTypes.Column, BuildingElement);
            ObjectType Stud = new ObjectType(ObjectTypes.Stud, Column);
            ObjectType Covering = new ObjectType(ObjectTypes.Covering, BuildingElement);
            ObjectType Opening = new ObjectType(ObjectTypes.Opening, BuildingElement);
            ObjectType Door = new ObjectType(ObjectTypes.Door, Opening);
            ObjectType Window = new ObjectType(ObjectTypes.Window, Opening);
            ObjectType Pile = new ObjectType(ObjectTypes.Pile, BuildingElement);
            ObjectType Plate = new ObjectType(ObjectTypes.Plate, BuildingElement);
            ObjectType Railing = new ObjectType(ObjectTypes.Railing, BuildingElement);
            ObjectType Ramp = new ObjectType(ObjectTypes.Ramp, BuildingElement);
            ObjectType RampFlight = new ObjectType(ObjectTypes.RampFlight, BuildingElement);
            ObjectType Roof = new ObjectType(ObjectTypes.Roof, BuildingElement);
            ObjectType ShadingDevice = new ObjectType(ObjectTypes.ShadingDevice, BuildingElement);
            ObjectType Slab = new ObjectType(ObjectTypes.Slab, BuildingElement);
            ObjectType CounterTop = new ObjectType(ObjectTypes. CounterTop, Slab);
            ObjectType Floor = new ObjectType(ObjectTypes.Floor, Slab);
            ObjectType Wall = new ObjectType(ObjectTypes.Wall, Slab);
            ObjectType CurtainWall = new ObjectType(ObjectTypes.CurtainWall, Wall);
            ObjectType Stair = new ObjectType(ObjectTypes.Stair, BuildingElement);
            ObjectType StairFlight = new ObjectType(ObjectTypes.StairFlight, BuildingElement);
            ObjectType FurnishingElement = new ObjectType(ObjectTypes.FurnishingElement, Real);
            ObjectType Couch = new ObjectType(ObjectTypes.Couch, FurnishingElement);
            ObjectType Chair = new ObjectType(ObjectTypes.Chair, FurnishingElement);
            ObjectType Table = new ObjectType(ObjectTypes.Table, FurnishingElement);
            ObjectType Bed = new ObjectType(ObjectTypes.Bed, FurnishingElement);
            ObjectType Plant = new ObjectType(ObjectTypes.Plant, FurnishingElement);
            ObjectType Cabinet = new ObjectType(ObjectTypes.Cabinet, FurnishingElement);
            ObjectType CornerCabinet = new ObjectType(ObjectTypes.CornerCabinet, Cabinet);
            ObjectType WallCabinet = new ObjectType(ObjectTypes.WallCabinet, Cabinet);
            ObjectType BaseCabinet = new ObjectType(ObjectTypes.BaseCabinet, Cabinet);
            ObjectType Shelf = new ObjectType(ObjectTypes.Shelf, FurnishingElement);
            ObjectType Container = new ObjectType(ObjectTypes.Container, FurnishingElement);
            ObjectType DistributionElement = new ObjectType(ObjectTypes.DistributionElement, Real);
            ObjectType DistributionControlElement = new ObjectType(ObjectTypes.DistributionControlElement, DistributionElement);
            ObjectType Alarm = new ObjectType(ObjectTypes.Alarm, DistributionControlElement);
            ObjectType Sensor = new ObjectType(ObjectTypes. Sensor, DistributionControlElement);
            ObjectType DistributionFlowElement = new ObjectType(ObjectTypes.DistributionFlowElement, DistributionElement);
            ObjectType DistributionChamberElement = new ObjectType(ObjectTypes.DistributionChamberElement, DistributionFlowElement);
            ObjectType FlowStorageDevice = new ObjectType(ObjectTypes.FlowStorageDevice, DistributionChamberElement);
            ObjectType ElectricFlowStorageDevice = new ObjectType(ObjectTypes.ElectricFlowStorageDevice, FlowStorageDevice);
            ObjectType Battery = new ObjectType(ObjectTypes.Battery, ElectricFlowStorageDevice);
            ObjectType Tank = new ObjectType(ObjectTypes.Tank, FlowStorageDevice);
            ObjectType FlowTerminal = new ObjectType(ObjectTypes.FlowTerminal, DistributionChamberElement);
            ObjectType AirTerminal = new ObjectType(ObjectTypes.AirTerminal, FlowTerminal);
            ObjectType Fan = new ObjectType(ObjectTypes.Fan, AirTerminal);
            ObjectType AudioVisualAppliance = new ObjectType(ObjectTypes.AudioVisualAppliance, FlowTerminal);
            ObjectType Radio = new ObjectType(ObjectTypes.Radio, AudioVisualAppliance);
            ObjectType Television = new ObjectType(ObjectTypes.Television, AudioVisualAppliance);
            ObjectType Speakers = new ObjectType(ObjectTypes.Speakers, AudioVisualAppliance);
            ObjectType CommunicationsAppliance = new ObjectType(ObjectTypes.CommunicationsAppliance, FlowTerminal);
            ObjectType Telephone = new ObjectType(ObjectTypes.Telephone, CommunicationsAppliance);
            ObjectType ElectricAppliance = new ObjectType(ObjectTypes.ElectricAppliance, FlowTerminal);
            ObjectType Range = new ObjectType(ObjectTypes.Range, ElectricAppliance);
            ObjectType Stove = new ObjectType(ObjectTypes.Stove, ElectricAppliance);
            ObjectType Oven = new ObjectType(ObjectTypes.Oven, ElectricAppliance);
            ObjectType CookTop = new ObjectType(ObjectTypes.CookTop, ElectricAppliance);
            ObjectType Refrigerator = new ObjectType(ObjectTypes.Refrigerator, ElectricAppliance);
            ObjectType Microwave = new ObjectType(ObjectTypes.Microwave, ElectricAppliance);
            ObjectType Dyer = new ObjectType(ObjectTypes.Dyer, ElectricAppliance);
            ObjectType FireSuppressionTerminal = new ObjectType(ObjectTypes.FireSuppressionTerminal, FlowTerminal);
            ObjectType LightFixture = new ObjectType(ObjectTypes.LightFixture, FlowTerminal);
            ObjectType Lamp = new ObjectType(ObjectTypes.Lamp, LightFixture);
            ObjectType MedicalDevice = new ObjectType(ObjectTypes.MedicalDevice, FlowTerminal);
            ObjectType Outlet = new ObjectType(ObjectTypes.Outlet, FlowTerminal);
            ObjectType SanitaryTerminal = new ObjectType(ObjectTypes.SanitaryTerminal, FlowTerminal);
            ObjectType Sink = new ObjectType(ObjectTypes.Sink, SanitaryTerminal);
            ObjectType Shower = new ObjectType(ObjectTypes.Shower, SanitaryTerminal);
            ObjectType Toilet = new ObjectType(ObjectTypes.Toilet, SanitaryTerminal);
            ObjectType Bath = new ObjectType(ObjectTypes.Bath, SanitaryTerminal);
            ObjectType WashingMashine = new ObjectType(ObjectTypes.WashingMashine, SanitaryTerminal);
            ObjectType SpaceHeater = new ObjectType(ObjectTypes.SpaceHeater, FlowTerminal);

            return Root;
        }

        private static Dictionary<ObjectTypes, ObjectType> traverse(ObjectType node)
        {
            Dictionary<ObjectTypes, ObjectType> d = new Dictionary<ObjectTypes, ObjectType>() { { node.ID, node } };
            node.Children.ToList().ForEach(c => d = d.Concat(traverse(c)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            return d;
        }
    }
}
