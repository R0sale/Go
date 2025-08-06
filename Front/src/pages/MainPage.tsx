import '../index.css'
import { useState } from 'react';
import Sidebar from '../components/SideBar';
import MapView from '../components/MapView';

function MainPage() {
  const [position, setPosition] = useState([55.751244, 37.618423]);
  const [isDimmed, setIsDimmed] = useState(false);

  return (
    <div className="h-screen flex pinned-left">
      <Sidebar onSearch={setPosition} isDimmed={isDimmed} />
      <MapView positionState={[position, setPosition]} state={[isDimmed, setIsDimmed]} />
    </div>
  );
}

export default MainPage;