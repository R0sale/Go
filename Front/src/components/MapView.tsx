import React, { Dispatch, SetStateAction } from 'react';
import { MapContainer, TileLayer, Marker, useMap } from 'react-leaflet';
import { Menu, ArrowBigRightDash } from 'lucide-react';
import 'leaflet/dist/leaflet.css';
import '../index.css';

interface MapViewProps {
    positionState: [number[], Dispatch<SetStateAction<number[]>>];
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

const getPosition = (): Promise<GeolocationPosition> => {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
        });
    };

const MapView: React.FC<MapViewProps> = ({ positionState, state }) => {
  const [isDimmed, setIsDimmed] = state;
  const [position, setPosition] = positionState;

  const handleDim = () => {
    setIsDimmed(!isDimmed);
  }

  const handlePosition = async () => {
    const { latitude, longitude } = (await getPosition()).coords;

    setPosition([latitude, longitude]);
  }

  return (
    <div 
      className={isDimmed ? 'grayscale blur cursor-pointer' : ''}
      onClick={isDimmed ? handleDim : () => {}}
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
        <div onClick={handlePosition} className='absolute w-10 h-10 z-[1000] items-center text-center mt-20 left-0 m-2 p-2 bg-white rounded-full cursor-pointer hover:bg-gray-200'>
          <ArrowBigRightDash />
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