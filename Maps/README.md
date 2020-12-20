# Map Blazor Library
This is a component library extract from the training about Blazing Pizza (https://github.com/dotnet-presentations/blazor-workshop/). Can be used to track orders in a map.

# How to use
Add this lines to the Index.html file, or in the html file you must be used the component.

<!DOCTYPE html>
<html>
    <head> 
        <!-- Reference to Maps Library-->
        <link href="_content/Maps/leaflet/leaflet.css" rel="stylesheet" />
    </head>
    <body>
        <!-- Reference to Maps Library-->
        <script src="_content/Maps/DeliveryMap.js"></script>
        <script src="_content/Maps/leaflet/leaflet.js"></script>
    </body>
</html>

Add that to the component where you will to use the map
<div class="map">
    <Map Markers="[List<Marker>]"/>
</div>    

where List<Marker> have 2 positions, start GPS position, and arrived GPS position. Can send only one, then don't show tracking.

# Implementation needed
You must be implemented the functionality to send List<Marker> from you app. Here have a simulate function to do this action.

