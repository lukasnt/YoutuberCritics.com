import React from "react";
import { Card, CardMedia, CardActionArea, Typography, CardHeader, CardContent, CardActions } from "@material-ui/core";
import Rating from "@material-ui/lab/Rating";
import { redirect } from "../../App";
import numeral from "numeral";

export default function ChannelCard( {channel} ) {

    function handleClick(event) {
        redirect("/channel/user?pathName=" + channel.youtubePath);
    }

    return (
        <Card style={{
            maxWidth: 285,
            maxHeight: 400,
            height: 400
        }}>
            <CardActionArea style={{height: 400}} onClick={handleClick}>
                <CardHeader
                    title= {
                        <div style={{height: 30, maxHeight: 30, overflow: "hidden"}}>
                            <Typography variant="h6"> {channel.title} </Typography>
                        </div>
                    }
                />
                <CardMedia
                    style={{
                        height: 140,
                        maxWidth: 500,
                        
                    }}
                    image={channel.imageURL}
                    title={channel.title}
                >
                    
                </CardMedia>
            <CardContent >
                <div style={{height: 60, maxHeight: 60, overflow: "hidden"}}>
                    <Typography variant="body2" color="textSecondary"> {channel.description} </Typography>
                </div>
                <br />
                <Typography variant="body2" color="textSecondary"> {numeral(channel.subscribers).format("0.0a") + " Subscribers"} </Typography>
            </CardContent>
            <CardActions>
                <Rating 
                    name="rating" 
                    style={{fontSize: "2.2rem"}} 
                    precision={0.1}
                    value={channel.ratingAverage} size="large" 
                    readOnly/>
                <CardActions>
                    <Typography variant="body2" color="textSecondary"> {channel.reviewCount + " Reviews"} </Typography>
                </CardActions>
            </CardActions>
            </CardActionArea>
        </Card>
    );
}