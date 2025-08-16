import type { LatLngExpression } from "leaflet";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";

interface FacilityMapProp {
    coords: LatLngExpression;
}

const FacilityMap: React.FC<FacilityMapProp> = ({ coords }) => {
    const eventHandler = {
        click: () => {
            console.log("HUI")
        }
    }

    return (
    <div>
        <MapContainer className="h-120 w-150.5" center={coords} zoom={13} scrollWheelZoom={true}>
            <TileLayer
                attribution='&copy; OpenStreetMap contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <Marker position={coords} eventHandlers={eventHandler}/>
        </MapContainer>
    </div>);
}


export default FacilityMap;