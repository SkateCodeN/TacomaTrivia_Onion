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

export function ConvertDayToNumber(day){
    switch(dayAsInteger.toLower()){
        
        case "sunday": 
            return 0;
        case "monday": 
            return 1;
        case "tuesday": 
            return 2;
        case "wednesday":
            return 3;
        case "thurday":
            return 4;
        case "friday":
            return 5;
        case "saturday":
            return 6;
        default:
            return "Error in interger passed to function"
    }
}