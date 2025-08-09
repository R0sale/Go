import '../index.css'
import { useState } from 'react';
import Sidebar from '../components/SideBar';
import MapView from '../components/MapView';
import type { Facility } from '../Facility';
import type { LatLngExpression } from 'leaflet';

function MainPage() {
  const [position, setPosition] = useState<LatLngExpression>({lat: 55.751244, lng: 37.618423});
  const [isDimmed, setIsDimmed] = useState(false);
  const [facilities, setFacilities] = useState<Facility[]>([]);
  const [map, setMap] = useState<L.Map | null>(null);

  return (
    <div className="h-screen flex pinned-left">
      <Sidebar onSearch={setPosition} isDimmed={isDimmed} setFacilities={setFacilities} map={map}/>
      <MapView positionState={[position, setPosition]} state={[isDimmed, setIsDimmed]} facilities={facilities} setMap={setMap}/>
    </div>
  );
}

export default MainPage;