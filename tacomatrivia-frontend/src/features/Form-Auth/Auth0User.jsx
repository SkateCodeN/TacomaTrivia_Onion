import axios from "axios";
import { useEffect, useState } from "react";
import { Group,Title } from "@mantine/core";
export default function Auth0User() {
    const [userData, setUserData] = useState([]);

    const getUserData = async () => {
        axios.get('/api/auth0/me')
            .then(response => setUserData(response.data))
            .catch(error => {
                if (error.response && error.response.status === 403) {
                    console.log("Caught the 401!");
                    setTest2("Unauthorized");
                } else {
                    console.error("Different error:", error.message);
                }
            });
    }

    useEffect(() =>{
        getUserData();
    },[])

    return(
        <Group>
            <Title p="lg" order={4}>
                { userData && `Name: ${userData.name}`}
            </Title>

            <Title order={5}>
                 { userData && `Role: ${userData.roles}`}
            </Title>
        </Group>
    )
}