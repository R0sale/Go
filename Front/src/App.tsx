import './index.css'
import { useState } from 'react';
import Sidebar from './SideBar';
import MapView from './MapView';

function App() {
  const [position, setPosition] = useState([55.751244, 37.618423]);

  return (
    <div className="h-screen flex pinned-left">
      <Sidebar onSearch={setPosition}/>
      <MapView position={position}/>
    </div>
  );
}

export default App;
