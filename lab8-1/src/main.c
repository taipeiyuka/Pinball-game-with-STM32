#include "stm32l476xx.h"

uint8_t input[100];

void GPIO_Init(){
	//UART
	RCC->AHB2ENR |= 0x1;//enable mode a
	GPIOA->MODER &= ~(0xF);
	GPIOA->MODER |= 0xA; //pin a0 a1
	GPIOA->AFR[0] |= 0x88; // both pin as AF8

	//I2C
	RCC->AHB2ENR |= 0x2;//enable pb clock
	GPIOB->MODER &= ~(0xF000);//pb6(scl) 7(sda)
	GPIOB->MODER |= 0xA000;
	GPIOB->OTYPER |= GPIO_OTYPER_OT6;
	GPIOB->OTYPER |= GPIO_OTYPER_OT7;
	GPIOB->OSPEEDR |= GPIO_OSPEEDR_OSPEED6;
	GPIOB->OSPEEDR |= GPIO_OSPEEDR_OSPEED7;
	GPIOB->PUPDR |= GPIO_PUPDR_PUPD6_0;
	GPIOB->PUPDR |= GPIO_PUPDR_PUPD7_0;
	GPIOB->AFR[0] |= 0x44000000; // both pin as AF4
}

void UART4_Init(){
	RCC->APB1ENR1 |= RCC_APB1ENR1_UART4EN;
	//RCC->CR |= RCC_CR_HSION;
	//RCC->CR |= RCC_CR_HSIKERON;
	//RCC->CCIPR |= RCC_CCIPR_UART4SEL_1;
	UART4->BRR |= 0x1A1; //usartdiv and brr under over16 0x1A1
	UART4->CR1 |= USART_CR1_UE;
	UART4->CR1 |= USART_CR1_RE;
}

void I2C1_Init(){
	RCC->APB1ENR1 |= RCC_APB1ENR1_I2C1EN;
	I2C1->CR1 &= ~I2C_CR1_PE;
	//standard mode 100kHz
	I2C1->TIMINGR &= ~I2C_TIMINGR_PRESC; //Presc = 0
	I2C1->TIMINGR |= (I2C_TIMINGR_SCLL & 0x13);//scll = 0x13
	I2C1->TIMINGR |= (I2C_TIMINGR_SCLH & 0xF00);//sclh = 0xF
	I2C1->TIMINGR |= (I2C_TIMINGR_SDADEL & 0x20000);//sdadel = 0x2
	I2C1->TIMINGR |= (I2C_TIMINGR_SCLDEL & (0x400000));//scldel = 0x4

	I2C1->CR1 |= I2C_CR1_PE;
}

void I2C_Transmit(uint8_t reg ,uint8_t data) {
	while(I2C1->ISR & I2C_ISR_BUSY);
	I2C1->CR2 &= ~I2C_CR2_NBYTES;
	I2C1->CR2 |= (I2C_CR2_NBYTES & (2<<16));
	I2C1->CR2 &= ~I2C_CR2_SADD;
	I2C1->CR2 |= 0x29<<1;//of BNO055
	I2C1->CR2 &= ~I2C_CR2_RD_WRN;//request a write

	I2C1->CR2 |= I2C_CR2_START;

	while(!(I2C1->ISR & I2C_ISR_TXIS));
	I2C1->TXDR = reg;
	while(!(I2C1->ISR & I2C_ISR_TXIS));
	I2C1->TXDR = data;

	while(!(I2C1->ISR & I2C_ISR_TC));
	I2C1->CR2 |= I2C_CR2_STOP;
	I2C1->ICR |= I2C_ICR_STOPCF;
}
void I2C_RequestRecieve(uint8_t reg ,uint8_t *arr, uint32_t size) {
	unsigned int i=0;
	while(I2C1->ISR & I2C_ISR_BUSY);
	I2C1->CR2 &= ~I2C_CR2_NBYTES;
	I2C1->CR2 |= (I2C_CR2_NBYTES & (1<<16));
	I2C1->CR2 &= ~I2C_CR2_SADD;
	I2C1->CR2 |= 0x29<<1;//of BNO055
	I2C1->CR2 &= ~I2C_CR2_RD_WRN;//request a write

	I2C1->CR2 |= I2C_CR2_START;

	while(!(I2C1->ISR & I2C_ISR_TXIS));
	I2C1->TXDR = reg;

	while(!(I2C1->ISR & I2C_ISR_TC));

	I2C1->CR1 &= ~I2C_CR1_PE;
	//restart
	I2C1->CR2 &= ~I2C_CR2_NBYTES;
	I2C1->CR2 |= (I2C_CR2_NBYTES & (size<<16));

	I2C1->CR2 |= I2C_CR2_RD_WRN;//request a read
	I2C1->CR1 |= I2C_CR1_PE;
	I2C1->CR2 |= I2C_CR2_START;

	for(i=0;i<size;i++){
		while(!(I2C1->ISR & I2C_ISR_RXNE));
		arr[i] = (uint8_t)(I2C1->RXDR & 0xFF);
	}
	while(!(I2C1->ISR & I2C_ISR_TC));
	I2C1->CR2 |= I2C_CR2_STOP;
	I2C1->ICR |= I2C_ICR_STOPCF;
}

void UART_Transmit(uint8_t *arr, uint32_t size) {
//TODO: Send str to UART and return how many bytes are successfully transmitted.
	unsigned int i=0;
	UART4->CR1 |= USART_CR1_TE;
	while(!(UART4->ISR & USART_ISR_TEACK));
	for(i=0;i<size;i++){
		while(!(UART4->ISR & USART_ISR_TXE));
		UART4->TDR = *arr;
		arr++;
	}
	while(!(UART4->ISR & USART_ISR_TC));
	UART4->CR1 &= ~USART_CR1_TE;
}

void BNO055_Init() {
	//set acc range to 8G
	I2C_Transmit(0x8, 0x0E);
	//set OPR_MODE to ACCONLY
	I2C_Transmit(0x3D, 0x1);
}

int main(){
	GPIO_Init();
	I2C1_Init();
	UART4_Init();
	BNO055_Init();
	int total = 0;
	while(1){
		I2C_RequestRecieve(0xD , &input[0], 1);
		I2C_RequestRecieve(0xC , &input[1], 1);
		int16_t a = (int16_t)(((int16_t)(input[0]<<8)) | input[1]);
		if(a>100){
			total += a;
		}else if(a>0){
			if(total>0) total += a;
		}else{
			if(total > 500){
				uint8_t str[5];
				str[4] = total%10+'0';
				total /= 10;
				str[3] = total%10+'0';
				total /= 10;
				str[2] = total%10+'0';
				total /= 10;
				str[1] = total%10+'0';
				total /= 10;
				str[0] = total%10+'0';
				total /= 10;
				UART_Transmit(str, 5);
			}
			total = 0;
		}
	}
}
