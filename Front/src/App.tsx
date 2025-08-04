import './index.css'
import { useState } from 'react';
import Sidebar from './SideBar';
import MapView from './MapView';

function App() {
  const [position, setPosition] = useState([55.751244, 37.618423]);
  const [isDimmed, setIsDimmed] = useState(false);

  return (
    <div className="h-screen flex pinned-left">
      <Sidebar onSearch={setPosition} isDimmed={isDimmed} />
      <MapView position={position} state={[isDimmed, setIsDimmed]} />
    </div>
  );
}

export default App;
