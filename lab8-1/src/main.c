#include "stm32l476xx.h"

void GPIO_Init(){
	//DBGMCU->CR |= (DBGMCU_CR_DBG_SLEEP|DBGMCU_CR_DBG_STOP|DBGMCU_CR_DBG_STANDBY);
	RCC->AHB2ENR |= 0x1;//enable mode a
	GPIOA->MODER &= ~(0xF);
	GPIOA->MODER |= 0xA; //pin a0 a1
	GPIOA->AFR[0] |= 0x88; // both pin as AF8
	RCC->AHB2ENR |= 0x4;//enable pc13 user button
	GPIOC->MODER &= ~(0x3<<26);//input
}
void interrupt_Init(){
	RCC->APB2ENR |= RCC_APB2ENR_SYSCFGEN;
	SYSCFG->EXTICR[3] |= SYSCFG_EXTICR4_EXTI13_PC;
	//set not masked
	EXTI->IMR1 |= EXTI_IMR1_IM13;
	//falling trigger (negative edge trigger)
	EXTI->FTSR1 |= EXTI_FTSR1_FT13;
	//cleared pending by writing 1
	if(EXTI_PR1_PIF13 & EXTI->PR1) EXTI->PR1 = EXTI_PR1_PIF13;
	NVIC->ICPR[1] |= 0x100;
	//enable EXTI15_10_IRQn = 40, External Line[15:10] Interrupts
	NVIC->ISER[1] |= 0x100;
}
void UART4_Init(){
	RCC->APB1ENR1 |= RCC_APB1ENR1_UART4EN;
	//RCC->CR |= RCC_CR_HSION;
	//RCC->CR |= RCC_CR_HSIKERON;
	//RCC->CCIPR |= RCC_CCIPR_UART4SEL_1;
	UART4->BRR |= (uint32_t)4000000/115200;//0x1A1; //usartdiv and brr under over16 0x1A1
	UART4->CR1 |= USART_CR1_UE;
	UART4->CR1 |= USART_CR1_RE;
}
void UART_Transmit(uint8_t *arr, uint32_t size) {
//TODO: Send str to UART and return how many bytes are successfully transmitted.
	int i=0;
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
void EXTI15_10_IRQHandler(void){
	uint8_t str[] = "Hello World!";
	UART_Transmit(str, 12);
	int delay = 50000;
	while(delay--);
	EXTI->PR1 = EXTI_PR1_PIF13; //clear pending
}
int main(){
	GPIO_Init();
	interrupt_Init();
	UART4_Init();
	while(1);
}
