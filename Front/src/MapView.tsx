import React from 'react';
import { MapContainer, TileLayer, Marker, useMap } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import './index.css';

interface MapViewProps {
    position: number[];
}

interface FlyToProps {
    position: number[];
}

function FlyTo({ position } : FlyToProps) {
  const map = useMap();
  map.flyTo(position, 13);
  return null;
}

const MapView: React.FC<MapViewProps> = ({ position }) => {
  return (
    <MapContainer
      center={position}
      zoom={13}
      scrollWheelZoom={true}
      className='h-screen w-screen'
    >
      <TileLayer
        attribution='&copy; OpenStreetMap contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      <Marker position={position} />
      <FlyTo position={position} />
    </MapContainer>
  );
}

export default MapView