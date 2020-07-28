import React from "react";
import Review from "../review/Review";
import { Grid } from "@material-ui/core";

export default function Reviews( {reviews, reviewMaxWidth} )
{
    reviews = reviews.map(r => <Grid item> <Review review={r} maxWidth={reviewMaxWidth} /> </Grid>)

    return (
        <Grid
            container 
            direction="row"
            justify="flex-start"
            alignItems="center" 
            spacing={2}>
            
            {reviews}
        </Grid>
    );
}