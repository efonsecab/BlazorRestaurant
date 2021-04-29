var mapWidget;
var currentPosition;
var mapOptions;
var mapElementId;
var mapsDataSource;
var isMapInitialized;
var assemblyName;
var onInitializedCallback;
var isLayerCreated = false;
var routeLayer = null;

export function InitializeMap(elementId, options, assembly, callback) {
    mapElementId = elementId;
    mapOptions = options;
    assemblyName = assembly;
    onInitializedCallback = callback;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(onCurrentPositionFound);
    }
}


function onCurrentPositionFound(position) {
    currentPosition = position;
    var configuredMapOptions =
    {
        center: [currentPosition.coords.longitude, currentPosition.coords.latitude],
        zoom: mapOptions.zoom,
        language: mapOptions.language,
        authOptions: mapOptions.authOptions
    };
    mapWidget = new atlas.Map(mapElementId, configuredMapOptions);
    isMapInitialized = true;
    DotNet.invokeMethodAsync(assemblyName, onInitializedCallback);
}

export function SetCamera(latitude, longitude) {
    mapWidget.setCamera({
        center: [longitude, latitude],
    });
}

export function RenderLine(startingPoint, finalPoint, pointsInRoute) {
    mapWidget.setCamera({
        center: [startingPoint.longitude, startingPoint.latitude],
    });
    if (mapsDataSource == null) {
        mapsDataSource = new atlas.source.DataSource();
        //Create a data source and add it to the map.
        mapWidget.sources.add(mapsDataSource);
    }
    else {
        mapsDataSource.clear();
    }
    var lineStart = startingPoint;
    if (pointsInRoute && pointsInRoute.length > 0) {
        pointsInRoute.forEach((element, index) => {
            var lineEnd = element;
            mapsDataSource.add(new atlas.data.LineString([[lineStart.longitude, lineStart.latitude], [element.longitude, element.latitude]]));
            lineStart = element;

        });
    }
    mapsDataSource.add(new atlas.data.LineString([[lineStart.longitude, lineStart.latitude], [finalPoint.longitude, finalPoint.latitude]]));
    //Create a line and add it to the data source.
    //mapsDataSource.add(new atlas.data.LineString([[startingPoint.longitude, startingPoint.latitude], [finalPoint.longitude, finalPoint.latitude]]));

    //Create a line layer to render the line to the map.
    if (routeLayer != null)
        mapWidget.layers.remove(routeLayer);
    routeLayer = new atlas.layer.LineLayer(mapsDataSource, null, {
        strokeColor: 'blue',
        strokeWidth: 5
    });
    mapWidget.layers.add(routeLayer);
}

export function SearchRoute(elementId, mapOptions, startingPoint, finalPoint, pointsInRoute) {
    debugger;
    if (!isMapInitialized) {
        throw 'Map is not initialized';
    }
    else {
        RenderLine(startingPoint, finalPoint, pointsInRoute);
    }
}