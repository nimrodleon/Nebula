<?php

namespace App\Command;

use App\Service\DataDirProvider;
use App\Util\JwtUtil;
use Symfony\Component\Console\Attribute\AsCommand;
use Symfony\Component\Console\Command\Command;
use Symfony\Component\Console\Input\InputArgument;
use Symfony\Component\Console\Input\InputInterface;
use Symfony\Component\Console\Input\InputOption;
use Symfony\Component\Console\Output\OutputInterface;
use Symfony\Component\Console\Style\SymfonyStyle;

#[AsCommand(
    name: 'generate:token',
    description: 'Generate a JWT token',
)]
class GenerateTokenCommand extends Command
{
    private JwtUtil $jwtUtil;
    private DataDirProvider $dataDirProvider;

    public function __construct(JwtUtil $jwtUtil, DataDirProvider $dataDirProvider)
    {
        parent::__construct();
        $this->jwtUtil = $jwtUtil;
        $this->dataDirProvider = $dataDirProvider;
    }

    protected function configure(): void
    {
        $this
            ->addArgument('user_id', InputArgument::REQUIRED, 'User ID')
            ->addOption('username', null, InputOption::VALUE_OPTIONAL, 'Username');
    }

    protected function execute(InputInterface $input, OutputInterface $output): int
    {
        $io = new SymfonyStyle($input, $output);
        $userId = $input->getArgument('user_id');
        $username = $input->getOption('username');

        $expiration = new \DateTime();
        $expiration->modify('+4 year');

        $payload = [
            'user_id' => $userId,
            'username' => $username,
            'exp' => $expiration->getTimestamp(),
        ];

        $token = $this->jwtUtil->generateToken($payload);
        $gestor = fopen($this->dataDirProvider->getDataDir() . DIRECTORY_SEPARATOR . "token.txt", "w");
        fwrite($gestor, $token);
        fclose($gestor);

        $io->success(sprintf('Generated JWT token: %s', $token));

        return Command::SUCCESS;
    }
}
