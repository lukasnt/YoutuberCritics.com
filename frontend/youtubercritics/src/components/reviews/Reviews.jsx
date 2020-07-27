import React from "react";
import Paper from "@material-ui/core/Paper";
import Typography from "@material-ui/core/Typography";
import Review from "../review/Review";
import { Grid } from "@material-ui/core";

export default function Reviews( {reviews, reviewMaxWidth} )
{
    reviews = reviews.map(r => <Grid item> <Review review={r} maxWidth={reviewMaxWidth} /> </Grid>)

    return (
        <div>
            <Paper elevation={2} style= {{maxWidth: "1300px", marginTop: 10, padding: 20}}>
            <Typography variant="h6"> Recent Reviews </Typography>
                <Grid
                    container 
                    direction="row"
                    justify="flex-start"
                    alignItems="center" 
                    spacing={2}>
                    
                    {reviews}
                </Grid>
            </Paper>
        </div>
    );
}