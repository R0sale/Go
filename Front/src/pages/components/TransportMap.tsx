import type { LatLngExpression } from "leaflet";
import { MapContainer, Marker, TileLayer } from "react-leaflet";
import { FlyTo } from "../../components/FlyTo";

interface TransportMapProp {
    coords: LatLngExpression;
}


const TransportMap: React.FC<TransportMapProp> = ({ coords }) => {
    return (
    <div>
        <MapContainer className="h-120" center={coords} zoom={13} scrollWheelZoom={true}>
            <TileLayer
                attribution='&copy; OpenStreetMap contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <Marker position={coords} />
            <FlyTo position={coords} />
        </MapContainer>
    </div>);
}


export default TransportMap;