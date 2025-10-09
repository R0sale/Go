import type { LatLngExpression } from "leaflet";
import { useEffect } from "react";
import { useMap } from "react-leaflet";


interface FlyToProps {
    position: LatLngExpression;
}

export function FlyTo({ position } : FlyToProps) {
  const map = useMap();

  useEffect(() => {
    map.flyTo(position, 13);
  }, [position, map]);

  return null;
}