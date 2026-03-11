
export default function ConvertToDateTime(date, time)
{
    const combinedString = `${date}T${time}`;

    const dateObj = new Date(combinedString);

    return dateObj;
}