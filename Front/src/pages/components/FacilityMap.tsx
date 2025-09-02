import type { LatLngExpression } from "leaflet";
import { MapContainer, Marker, TileLayer } from "react-leaflet";

interface FacilityMapProp {
    coords: LatLngExpression;
}

const FacilityMap: React.FC<FacilityMapProp> = ({ coords }) => {

    return (
    <div>
        <MapContainer className="h-120 w-150.5" center={coords} zoom={13} scrollWheelZoom={true}>
            <TileLayer
                attribution='&copy; OpenStreetMap contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <Marker position={coords}/>
        </MapContainer>
    </div>);
}


export default FacilityMap;