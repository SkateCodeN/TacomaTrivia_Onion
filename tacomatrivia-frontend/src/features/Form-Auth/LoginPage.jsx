import {
    Container,
    Stack,
    Button,
    Title
} from '@mantine/core';
import LoginForm from "./LoginForm";
export default function LoginPage() 
{
    const handleClick = () =>{
        window.location.href ='http://localhost:5067/api/auth0';
    }
    return(
        <Container  p="lg" size={500}>
            <LoginForm />

            <Stack p="lg">
                <Title order={3}>Auth0 Test</Title>
                <Button onClick={handleClick}>Login</Button>
            </Stack>
        </Container>

        
    );
}