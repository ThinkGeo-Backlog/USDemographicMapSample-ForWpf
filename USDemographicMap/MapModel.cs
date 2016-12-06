using System.Windows;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.Wpf;

namespace ThinkGeo.MapSuite.USDemographicMap
{
    public class MapModel
    {
        private static RectangleShape globeExtent = new RectangleShape(-128.090753125, 58.27777578125, -68.149346875, 19.10492421875);

        private WpfMap mapControl;
        private LayerOverlay demographicLayerOverlay;
        private LegendAdornmentLayer legendAdornmentLayer;
        private HighlightOverlay highlightOverlay;
        private ShapeFileFeatureLayer censusStateFeatureLayer;
        private ShapeFileFeatureLayer customDataFeatureLayer;

        public MapModel()
            : this(null)
        { }

        public MapModel(WpfMap wpfMap)
        {
            mapControl = wpfMap;

            InitializeMap();
            InitializeEvents();
        }

        public WpfMap MapControl
        {
            get { return mapControl; }
            set { mapControl = value; }
        }

        public HighlightOverlay HighlightOverlay
        {
            get { return highlightOverlay; }
        }

        public LegendAdornmentLayer LegendAdornmentLayer
        {
            get { return legendAdornmentLayer; }
        }

        public LayerOverlay DemographicLayerOverlay
        {
            get { return demographicLayerOverlay; }
        }

        public ShapeFileFeatureLayer DefaultFeatureLayer
        {
            get { return (ShapeFileFeatureLayer)demographicLayerOverlay.Layers["DefaultFeatureLayer"]; }
            set { demographicLayerOverlay.Layers["DefaultFeatureLayer"] = value; }
        }

        public ShapeFileFeatureLayer CustomDataFeatureLayer
        {
            get { return customDataFeatureLayer; }
            set { customDataFeatureLayer = value; }
        }

        public ShapeFileFeatureLayer CensusStateFeatureLayer
        {
            get { return censusStateFeatureLayer; }
        }

        public string LegendTitle
        {
            get { return legendAdornmentLayer.Title.TextStyle.TextColumnName; }
            set { legendAdornmentLayer.Title.TextStyle.TextColumnName = value; }
        }

        private void InitializeMap()
        {
            mapControl.MapTools.Logo.IsEnabled = true;
            mapControl.MapTools.Logo.Margin = new Thickness(0, 0, 10, 5);
            mapControl.MapTools.MouseCoordinate.IsEnabled = true;
            mapControl.MapTools.MouseCoordinate.Visibility = Visibility.Hidden;
            mapControl.MapTools.MouseCoordinate.Margin = new Thickness(0, 0, 100, 5);
            mapControl.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.Custom;

            mapControl.Overlays.Add(new WorldMapKitWmsWpfOverlay());

            censusStateFeatureLayer = new ShapeFileFeatureLayer(MapSuiteSampleHelper.GetValueFromConfiguration("UsShapefilePath"));
            censusStateFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            demographicLayerOverlay = new LayerOverlay();
            demographicLayerOverlay.TileType = TileType.SingleTile;
            mapControl.Overlays.Add(demographicLayerOverlay);

            highlightOverlay = new HighlightOverlay();
            mapControl.Overlays.Add(highlightOverlay);

            legendAdornmentLayer = new LegendAdornmentLayer();
            legendAdornmentLayer.Location = AdornmentLocation.LowerLeft;
            legendAdornmentLayer.Title = new LegendItem();
            legendAdornmentLayer.Title.ImageJustificationMode = LegendImageJustificationMode.JustifyImageRight;
            legendAdornmentLayer.Title.TopPadding = 10;
            legendAdornmentLayer.Title.BottomPadding = 10;
            legendAdornmentLayer.Title.TextStyle = new TextStyle("Population", new GeoFont("Segoe UI", 12), new GeoSolidBrush(GeoColor.SimpleColors.Black));
            mapControl.AdornmentOverlay.Layers.Add(legendAdornmentLayer);

            DefaultFeatureLayer = censusStateFeatureLayer;

            MapControl.CurrentExtent = globeExtent;
        }

        private void InitializeEvents()
        {
            MapControl.MapTools.PanZoomBar.GlobeButtonClick += PanZoomBar_GlobeButtonClick;
        }

        private void PanZoomBar_GlobeButtonClick(object sender, GlobeButtonClickPanZoomBarMapToolEventArgs e)
        {
            e.NewExtent = globeExtent;
        }
    }
}
