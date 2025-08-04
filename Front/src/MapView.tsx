import React, { Dispatch, SetStateAction } from 'react';
import { MapContainer, TileLayer, Marker, useMap } from 'react-leaflet';
import { Menu } from 'lucide-react';
import 'leaflet/dist/leaflet.css';
import './index.css';

interface MapViewProps {
    position: number[];
    state: [boolean, Dispatch<SetStateAction<boolean>>];
}

interface FlyToProps {
    position: number[];
}

function FlyTo({ position } : FlyToProps) {
  const map = useMap();
  map.flyTo(position, 13);
  return null;
}


const MapView: React.FC<MapViewProps> = ({ position, state }) => {
  const [isDimmed, setIsDimmed] = state;

  const handleDim = () => {
    setIsDimmed(!isDimmed);
  }

  return (
    <div 
      className={isDimmed ? 'grayscale blur cursor-pointer' : ''}
      onClick={isDimmed && handleDim}
      >
      <MapContainer
        center={position}
        zoom={13}
        scrollWheelZoom={true}
        className='h-screen w-100 z-0'
      >
        <div onClick={handleDim} className='absolute w-10 h-10 z-[1000] items-center text-center right-0 m-2 p-2 bg-white rounded-full cursor-pointer hover:bg-gray-200'>
          <Menu />
        </div>
        <TileLayer
          attribution='&copy; OpenStreetMap contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <Marker position={position} />
        <FlyTo position={position} />
      </MapContainer>
    </div>
  );
}

export default MapView