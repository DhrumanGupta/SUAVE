import React from 'react';
import styles from '../stylesheets/Home.module.css';
import hero from '../images/hero.png';
import tinyHero from '../images/tiny/hero.png';
import {useSpring, animated, config} from "react-spring";
import {Link} from "react-router-dom";
import ProgressiveImage from "react-progressive-image";

function Home() {
	const animatedStyles = useSpring({
		config: config.gentle,
		from: {transform: "translateY(-20vh)", opacity: 0},
		to: {transform: "translateY(0%)", opacity: 1},
	});
	
	return (
		<div className={styles.container}>
			<header>
				<nav className={styles.navbar}>
					<ul>
						<li>
							<Link to={'/search'}>Use Online</Link>
						</li>
						<li>
							<a href={'https://discord.com/api/oauth2/authorize?client_id=868771461840637953&permissions=8&scope=bot'} target={'_blank'} rel="noreferrer">Discord Bot</a>
						</li>
						<li>
							<a href={'https://github.com/DhrumanGupta/SUAVE/releases'} target={'_blank'} rel="noreferrer">AI Search</a>
						</li>
						<li>
							<a href={'/swagger'} target={'_blank'} rel="noreferrer">Docs</a>
						</li>
					</ul>
				</nav>
			</header>

			<ProgressiveImage src={hero} placeholder={tinyHero}>
				{(src) => <img src={src} alt={'hero'} className={styles.hero}/>}
			</ProgressiveImage>
			
			<animated.div className={styles.textContainer} style={animatedStyles}>
				<h1 className={styles.title}>SUAVE</h1>
				<h3 className={styles.subtitle}>Search and Use APIs Very Easily</h3>
			</animated.div>
		</div>
	);
}

export default Home;