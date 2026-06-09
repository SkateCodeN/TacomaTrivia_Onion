export function DayConverter(dayAsInteger){
    switch(dayAsInteger){
        case null:
            return ""
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

export function ConvertDayToNumber(dayAsInteger){
    switch(dayAsInteger){
        
        case "Sunday": 
            return '0';
        case "Monday": 
            return '1';
        case "Tuesday": 
            return '2';
        case "Wednesday":
            return '3';
        case "Thurday":
            return '4';
        case "Friday":
            return '5';
        case "Saturday":
            return '6';
        default:
            return "Error in interger passed to function"
    }
}

