//Become a mod call to action
import { Container, Grid, SimpleGrid, Skeleton } from '@mantine/core';
import moderatorPic from "@assets/shkrabaanthony-mod.jpg"

export default function BecomeModCTA() {
    const PRIMARY_COL_HEIGHT = '300px';
    const SECONDARY_COL_HEIGHT = `calc(${PRIMARY_COL_HEIGHT} / 2 - var(--mantine-spacing-md) / 2)`;
    
    return (
        <>
            <Container my="md">
                <SimpleGrid cols={{ base: 1, sm: 2 }} spacing="md">
                    {/* <Skeleton height={PRIMARY_COL_HEIGHT} radius="md" animate={false} /> */}
                    <img src={moderatorPic} width={400}/>
                    <Grid gutter="md">
                        <Grid.Col>
                            <h1>Become a moderator!</h1>
                        </Grid.Col>
                        <Grid.Col span={12}>
                            <p>Help me maintain the venues list by becoming a moderator, all I ask is that 
                                you register and send me an email. From there you will have control over the 
                                Tacoma Trivia list!
                            </p>
                            <button>Request</button>
                        </Grid.Col>
                        
                    </Grid>
                </SimpleGrid>
            </Container>

        </>
    )
}