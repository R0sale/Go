import '../index.css'
import { useState } from 'react';
import Sidebar from '../components/SideBar';
import MapView from '../components/MapView';
import type { Facility } from '../models/Facility';
import type { LatLngExpression } from 'leaflet';
import type { Vehicle } from '../models/Transport';

function MainPage() {
  const [position, setPosition] = useState<LatLngExpression>({lat: 55.751244, lng: 37.618423});
  const [isDimmed, setIsDimmed] = useState(false);
  const [facilities, setFacilities] = useState<Facility[]>([]);
  const [map, setMap] = useState<L.Map | null>(null);
  const [transport, setTransport] = useState<Vehicle[]>([]);

  return (
    <div className="h-screen flex pinned-left">
      <Sidebar onSearch={setPosition} isDimmed={isDimmed} setFacilities={setFacilities} map={map} setTransport={setTransport}/>
      <MapView positionState={[position, setPosition]} state={[isDimmed, setIsDimmed]} facilities={facilities} transport={transport} setMap={setMap}/>
    </div>
  );
}

export default MainPage;