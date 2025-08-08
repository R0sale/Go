import { Bed, Plus, Coffee, ShoppingBag, Dumbbell, CreditCard, Fuel, Wrench, Hospital, Landmark, Scissors, Utensils } from 'lucide-react';
import type { Facility } from '../Facility';

const queryFactory = (key: string, value: string, bounds: number[]) => {
    if (value === '*')
        return (`
<osm-script output="json" timeout="25">
    <query type="node">
        <bbox-query s="${bounds[0]}" w="${bounds[1]}" n="${bounds[2]}" e="${bounds[3]}" />
        <has-kv k="${key}" regv="${value}" />
    </query>
    <print/>
</osm-script>
`)
    else
        return (`
<osm-script output="json" timeout="25">
    <query type="node">
        <bbox-query s="${bounds[0]}" w="${bounds[1]}" n="${bounds[2]}" e="${bounds[3]}" />
        <has-kv k="${key}" regv="${value}" />
    </query>
    <print limit="100"/>
</osm-script>
`)}

const categories = [
  { label: 'Where to eat', icon: <Utensils className='w-5 h-5'/>, key: 'amenity', value: 'cafe|restaurant|fast_food'},
  { label: 'Products', icon: <ShoppingBag className="w-5 h-5" />, key: 'shop', value: ''},
  { label: 'Hotels', icon: <Bed className="w-5 h-5" />, key: 'tourism', value: 'hotel|motel|guest_house' },
  { label: 'Pharmacy', icon: <Plus className="w-5 h-5" />, key: 'amenity', value: 'pharmacy' },
  { label: 'Cafe', icon: <Coffee className="w-5 h-5" />, key: 'amenity', value: 'cafe' },
  { label: 'Shopping malls', icon: <ShoppingBag className="w-5 h-5" />, key: 'shop', value: 'mall' },
  { label: 'Sport', icon: <Dumbbell className="w-5 h-5" />, key: 'leisure', value: 'sports_centre|stadium|fitness_centre' },
  { label: 'ATM', icon: <CreditCard className="w-5 h-5" />, key: 'amenity', value: 'atm' },
  { label: 'Fuel', icon: <Fuel className="w-5 h-5" />, key: 'amenity', value: 'fuel' },
  { label: 'Auto Services', icon: <Wrench className="w-5 h-5" />, key: 'amenity', value: 'car_repair|car_wash|car_rental' },
  { label: 'Hospital', icon: <Hospital className='w-5 h-5' />, key: 'amenity', value: 'hospital'},
  { label: 'Museum', icon: <Landmark className='w-5 h-5' />, key: 'tourism', value: 'museum'},
  { label: 'Beauty Salon', icon: <Scissors className='w-5 h-5' />, key: 'shop', value: 'beauty'},
];

interface CategoriesProps {
    isVisible: boolean;
    setFacilities: (facilities: Facility[]) => void;
    map: L.Map | null;
}

const Categories: React.FC<CategoriesProps> = ({isVisible, setFacilities, map}) => {

    const handleClick = (key:string, value:string) => {
        if (map != null)
        {
            const coords = map.getBounds();
            const _southWest = coords.getSouthWest();
            const _northEast = coords.getNorthEast();
            const bounds = [_southWest.lat, _southWest.lng, _northEast.lat, _northEast.lng];

            const query = queryFactory(key, value, bounds);

            fetch("https://overpass-api.de/api/interpreter", {
                method: "POST",
                body: query,
                headers: {
                    "Content-Type": "text/plain"
                }
                })
                .then(res => res.json())
                .then(data => {
                    const facilities = data.elements.map((el: any) => ({
                        id: el.id,
                        name: el.tags?.name || 'Unknown',
                        lat: el.lat,
                        lon: el.lon
                    }));
                    setFacilities(facilities);
                })
                .catch(err => console.error(err));
        }
    }


    return (
        <div className="grid grid-cols-5 gap-3 text-center text-xs mt-3 p-4">
            {categories.map((cat, i) => {
                if (i < 10)
                    return <div key={i} onClick={() => {handleClick(cat.key, cat.value)}} className="flex flex-col items-center justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                    {cat.icon}
                    <span className="mt-1">{cat.label}</span>
                    </div>
                else
                    return (!isVisible && <div key={i} onClick={() => {handleClick(cat.key, cat.value)}} className="flex flex-col items-center justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                    {cat.icon}
                    <span className="mt-1">{cat.label}</span>
                    </div>)
            })}
        </div>
    );
}

export default Categories;