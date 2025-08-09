import React, { useEffect } from 'react';
import type { Dispatch, SetStateAction } from 'react';
import { MapContainer, TileLayer, Marker, useMap } from 'react-leaflet';
import { Menu, ArrowBigRightDash } from 'lucide-react';
import 'leaflet/dist/leaflet.css';
import '../index.css';
import type { Facility } from '../Facility';
import L, { type LatLngExpression } from 'leaflet';

interface MapViewProps {
    positionState: [LatLngExpression, Dispatch<SetStateAction<LatLngExpression>>];
    state: [boolean, Dispatch<SetStateAction<boolean>>];
    facilities: Facility[];
    setMap: Dispatch<SetStateAction<L.Map | null>>
}

interface FlyToProps {
    position: LatLngExpression;
}

interface ProvideMapProps {
    setMap: Dispatch<SetStateAction<L.Map | null>>;
}

function FlyTo({ position } : FlyToProps) {
  const map = useMap();

  useEffect(() => {
    map.flyTo(position, 13);
  }, [position, map]);

  return null;
}

function ProvideMap({setMap} : ProvideMapProps) {
  const map = useMap();
  useEffect(() => {
    setMap(map);
  }, [map, setMap]);
  return null;
}

const getPosition = (): Promise<GeolocationPosition> => {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
        });
    };

const MapView: React.FC<MapViewProps> = ({ positionState, state, facilities, setMap}) => {
  const [isDimmed, setIsDimmed] = state;
  const [position, setPosition] = positionState;

  const handleDim = () => {
    setIsDimmed(!isDimmed);
  }

  const handlePosition = async () => {
    const { latitude, longitude } = (await getPosition()).coords;

    setPosition({lat: latitude, lng: longitude});
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
        {facilities.length > 0 && facilities.map((elem) => {
          return <Marker position={[elem.lat, elem.lon]} />
        })}
        <Marker position={position} />
        <FlyTo position={position} />
        <ProvideMap setMap={setMap}/>
      </MapContainer>
    </div>
  );
}

export default MapView