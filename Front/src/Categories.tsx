import { Bed, Plus, Coffee, ShoppingBag, Dumbbell, CreditCard, Fuel, Wrench, Hospital, Landmark, Scissors, Utensils } from 'lucide-react';
import React from 'react';

const categories = [
  { label: 'Where to eat', icon: <Utensils className='w-5 h-5'/> },
  { label: 'Products', icon: <ShoppingBag className="w-5 h-5" /> },
  { label: 'Hotels', icon: <Bed className="w-5 h-5" /> },
  { label: 'Pharmacy', icon: <Plus className="w-5 h-5" /> },
  { label: 'Cafe', icon: <Coffee className="w-5 h-5" /> },
  { label: 'Shopping malls', icon: <ShoppingBag className="w-5 h-5" /> },
  { label: 'Sport', icon: <Dumbbell className="w-5 h-5" /> },
  { label: 'ATM', icon: <CreditCard className="w-5 h-5" /> },
  { label: 'Fuel', icon: <Fuel className="w-5 h-5" /> },
  { label: 'Auto Services', icon: <Wrench className="w-5 h-5" /> },
  { label: 'Hospital', icon: <Hospital className='w-5 h-5' />},
  { label: 'Museum', icon: <Landmark className='w-5 h-5' />},
  { label: 'Beauty Salon', icon: <Scissors className='w-5 h-5' />},
];

interface CategoriesProps {
    isVisible: boolean;
}

const Categories: React.FC<CategoriesProps> = ({isVisible}) => {
    return (
        <div className="grid grid-cols-5 gap-3 text-center text-xs mt-3 p-4">
            {categories.map((cat, i) => {
                if (i < 10)
                    return <div key={i} className="flex flex-col items-center justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                    {cat.icon}
                    <span className="mt-1">{cat.label}</span>
                    </div>
                else
                    return (!isVisible && <div key={i} className="flex flex-col items-center justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                    {cat.icon}
                    <span className="mt-1">{cat.label}</span>
                    </div>)
            })}
        </div>
    );
}

export default Categories;