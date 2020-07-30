import React, { useState } from "react";
import axios from "axios";
import Reviews from "./Reviews";
import { backendDomain } from "../../App";
import { Paper, Typography, Grid, CircularProgress } from "@material-ui/core";

export default function RecentReviews( )
{
    const [sent, setSent] = useState(false);
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(
        <Grid item xs={12} size="large" style={{display: "flex", justifyContent: "center"}}>
            <CircularProgress style={{
                width: "150px",
                height: "150px",
            }} color="secondary"/> 
        </Grid>
    );

    if (!sent) {
        axios.get(backendDomain + "/api/reviews/recent")
            .then(res => {
                setReviews(res.data);
                setLoading(null);
            });
        setSent(true);
    }

    return (
        <Paper elevation={2} style= {{maxWidth: "1300px", marginTop: 10, padding: 20}}>
            <Typography variant="h6"> Recent Reviews </Typography>
            {loading}
            <Reviews reviews={reviews} reviewMaxWidth={"585px"} />
        </Paper>
    );
}