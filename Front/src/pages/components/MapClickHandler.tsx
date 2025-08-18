import type { LatLng } from "leaflet";
import { useMapEvents } from "react-leaflet";

interface MapClickHandlerProps {
    onClick: (coords: LatLng) => void;
}

const MapClickHandler: React.FC<MapClickHandlerProps> = ({ onClick }) => {
    useMapEvents({
        click(e) {
            console.log(e.latlng);

            onClick(e.latlng);
        }
    });

    return null;
}

export default MapClickHandler;