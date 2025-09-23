import type { LatLngExpression } from "leaflet";
import { MapContainer, Polyline, TileLayer } from "react-leaflet";
import type { Node } from "../../models/Node";

interface RouteMapProp {
    nodes: Node[];
    coords: LatLngExpression;
}

const RouteMap: React.FC<RouteMapProp> = ({ nodes, coords }) => {
    const polylinePoints: [number, number][] = nodes.map(n => [n.latitude, n.longitude]);

    return (
    <div>
        <MapContainer className="h-120 w-150.5" center={coords} zoom={13} scrollWheelZoom={true}>
            <TileLayer
                attribution='&copy; OpenStreetMap contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <Polyline positions={polylinePoints}/>
        </MapContainer>
    </div>);
}


export default RouteMap;