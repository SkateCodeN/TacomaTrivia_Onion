export default function DayConverter(dayAsInteger){
    switch(dayAsInteger){
        case null:
            return "No DB data"
        case 0: 
            return "Sunday";
        case 1: 
            return "Monday"
        case 2: 
            return "Tuesday";
        case 3:
            return "Wednesday";
        case 4:
            return "Thurday";
        case 5:
            return "Friday";
        case 6:
            return "Saturday"
        default:
            return "Error in interger passed to function"
    }
}