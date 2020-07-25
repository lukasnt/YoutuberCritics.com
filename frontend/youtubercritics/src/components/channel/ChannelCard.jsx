import React, { useState } from "react";
import { Card, CardMedia, CardActionArea, Typography, CardHeader, CardContent, CardActions } from "@material-ui/core";
import Rating from "@material-ui/lab/Rating";
import { frontendDomain } from "../../App";

export default function ChannelCard( {channel} ) {
    const [redirect] = useState(null);

    function handleClick(event) {
        
        window.location.replace(frontendDomain + "/channel/user?pathName=" + channel.youtubePath);
    }

    return (
        <Card style={{
            maxWidth: 285,
        }}>
            <CardActionArea style={{height: 400}} onClick={handleClick}>
                <CardHeader
                    title= {
                        <Typography variant="h6"> {channel.title} </Typography>
                    }
                />
                <CardMedia
                    style={{
                        height: 140,
                        maxWidth: 500,
                    }}
                    image={channel.imageURL}
                    title={channel.title}
                />
            <CardContent >
                <Typography variant="body2" color="textSecondary"> {channel.description} </Typography>
            </CardContent>
            <CardActions>
                <Rating 
                    name="rating" 
                    style={{fontSize: "2.2rem"}} 
                    precision={0.1}
                    value={3.4} size="large" 
                    readOnly/>
                <CardActions>
                    <Typography variant="body2" color="textSecondary"> 561 reviews </Typography>
                </CardActions>
            </CardActions>
            </CardActionArea>
            {redirect}
        </Card>
    );
}