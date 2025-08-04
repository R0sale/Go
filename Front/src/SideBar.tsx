import React from 'react';
import { useState } from 'react';
import './index.css';
import SearchBar from './SearchBar';
import Categories from './Categories';
import Menu from './Menu';

interface SideBarProps {
    onSearch: (coords: [number, number]) => void;
    isDimmed: boolean;
}

const Sidebar: React.FC<SideBarProps> = ({onSearch, isDimmed}) => {
    const [isVisible, setIsVisible] = useState(true);

    const handleVisible = () => {
        setIsVisible(false);
    }

    return (
        <div className="w-200 max-w-md bg-white rounded-xl shadow-md space-y-4">
            <div className='p-4'>
                <SearchBar onSearch={onSearch}/>
            </div>

            {!isDimmed ? (
            <div>
                <div className="flex items-center justify-between h-10 p-4">
                    <div className="text-lg font-semibold">City <span className="text-sm">☀️ ??°C</span></div>
                    {isVisible && <button onClick={handleVisible} className="text-blue-500 text-sm">All places</button>}
                </div>

                <Categories isVisible={isVisible}/>
            </div>
            ) : <Menu />}
        </div>
    );
};

export default Sidebar;
