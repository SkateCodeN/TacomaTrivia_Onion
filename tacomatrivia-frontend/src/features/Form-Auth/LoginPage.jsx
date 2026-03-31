import {
    Container
} from '@mantine/core';
import LoginForm from "./LoginForm";
export default function LoginPage() 
{
    return(
        <Container  p="lg" size={500}>
            <LoginForm />
        </Container>
    );
}